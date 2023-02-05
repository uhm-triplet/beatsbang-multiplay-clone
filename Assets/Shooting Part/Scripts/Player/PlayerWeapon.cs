using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerWeapon : NetworkBehaviour
{
    bool fDown;
    bool rDown;
    bool gDown;
    bool isFireReady;
    [HideInInspector] public bool isReloading;
    float fireDelay;

    public GameObject grenadeObj;
    [SerializeField] Transform grenadePos;
    [SerializeField] private List<GameObject> spawnedGrenades = new List<GameObject>();


    [SerializeField] Transform bulletPos;
    [SerializeField] private List<GameObject> spawnedBullets = new List<GameObject>();
    public int maxAmmo = 50;
    public int curAmmo = 50;



    Animator animator;
    PlayerItem playerItem;
    PlayerMove playerMove;
    PlayerAim playerAim;
    public override void OnNetworkSpawn()
    {
        playerMove = GetComponent<PlayerMove>();
        playerItem = GetComponent<PlayerItem>();
        playerAim = GetComponent<PlayerAim>();
        animator = GetComponentInChildren<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (playerItem.isDead) return;
        GetInput();
        Attack();
        Grenade();
        Reload();
    }

    void GetInput()
    {
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Grenade");
        rDown = Input.GetButtonDown("Reload");
    }

    void Attack()
    {
        // equipWeapon = GameObject.Find("Player").GetComponent<PlayerItem>().equipWeapon;
        if (playerItem.equipWeapon == null || playerItem.equipWeapon.curAmmo == 0) return;

        fireDelay += Time.deltaTime;
        isFireReady = playerItem.equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isReloading && !playerMove.isDodge)
        {
            if (playerItem.equipWeapon.type == Weapon.Type.Melee)
            {
                animator.SetTrigger("doSwing");
                // Swing();
                Swing();
                fireDelay = 0;
            }
            else if (playerItem.equipWeapon.type == Weapon.Type.Range && curAmmo > 0)
            {
                animator.SetTrigger("doShot");
                Shot();
                playerItem.equipWeapon.curAmmo--;
                fireDelay = 0;
            }
        }
    }

    private void Swing()
    {
        SwingServerRpc();
        if (IsServer)
        {
            SwingClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SwingServerRpc()
    {

        StopCoroutine("SwingC");
        StartCoroutine("SwingC");
    }

    [ClientRpc]
    private void SwingClientRpc()
    {
        StopCoroutine("SwingC");
        StartCoroutine("SwingC");
    }

    IEnumerator SwingC()
    {
        animator.SetTrigger("doSwing");
        yield return new WaitForSeconds(0.01f);
        playerItem.equipWeapon.meleeArea.enabled = true;
        playerItem.equipWeapon.trailEffect.enabled = true;

        yield return new WaitForSeconds(0.01f);
        playerItem.equipWeapon.meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        playerItem.equipWeapon.trailEffect.enabled = false;
    }

    private void Shot()
    {
        ShotServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotServerRpc()
    {
        animator.SetTrigger("doShot");
        bulletPos.LookAt(playerAim.aimPos);
        GameObject instantBullet = Instantiate(playerItem.equipWeapon.bullet, bulletPos.position, bulletPos.rotation);
        spawnedBullets.Add(instantBullet);
        instantBullet.GetComponent<Bullet>().parent = this;
        instantBullet.GetComponent<NetworkObject>().Spawn();
        if (IsServer)
        {
            ShotClientRpc();
        }
    }

    [ClientRpc]
    private void ShotClientRpc()
    {
        animator.SetTrigger("doShot");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc()
    {
        GameObject toDestroy = spawnedBullets[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBullets.Remove(toDestroy);
        Destroy(toDestroy);
    }

    void Grenade()
    {
        if (playerItem.hasGrenades == 0) return;
        if (gDown && !isReloading)
        {
            GrenadeServerRpc();
        }
    }

    [ServerRpc]
    private void GrenadeServerRpc()
    {
        grenadePos.LookAt(playerAim.aimPos);
        GameObject instantGrenade = Instantiate(grenadeObj, grenadePos.position, grenadePos.rotation);
        spawnedGrenades.Add(instantGrenade);
        instantGrenade.GetComponent<Grenade>().parent = this;
        instantGrenade.GetComponent<NetworkObject>().Spawn();

        playerItem.hasGrenades -= 1;
        playerItem.grenades[playerItem.hasGrenades].SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyGrenadeServerRpc()
    {
        GameObject toDestroy = spawnedGrenades[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedGrenades.Remove(toDestroy);
        Destroy(toDestroy);
    }

    void Reload()
    {
        if (playerItem.equipWeapon == null) return;

        if (playerItem.equipWeapon.type == Weapon.Type.Melee) return;

        if (playerItem.ammo == 0) return;

        if (rDown && isFireReady && !isReloading)
        {
            animator.SetTrigger("doReload");
            isReloading = true;
            Debug.Log("Reload");

            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        int requiredAmmo = playerItem.equipWeapon.maxAmmo - playerItem.equipWeapon.curAmmo;
        int reAmmo = playerItem.ammo < requiredAmmo ? playerItem.ammo : requiredAmmo;
        playerItem.equipWeapon.curAmmo += reAmmo;
        playerItem.ammo -= reAmmo;
        isReloading = false;
    }


}
