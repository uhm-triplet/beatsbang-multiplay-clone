using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MPlayerState : NetworkBehaviour
{
    public GameObject[] weapons;
    public int hasWeapon = -1;

    public GameObject[] grenades;
    public int hasGrenades;
    public int ammo;
    public int health;

    public int maxAmmo;
    public int maxHealth;
    public int maxHasGrenades;

    public int score;

    bool isDamage;

    GameObject nearObject;
    public MWeapon equipWeapon;
    // public Weapon equipWeapon2;
    Animator animator;
    MeshRenderer[] meshs;
    Rigidbody rigid;

    Vector3 impact = Vector3.zero;
    private CharacterController controller;


    bool oneDown;
    bool twoDown;
    bool threeDown;

    public override void OnNetworkSpawn()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (!IsOwner) return;
        impact.z = -50;
        rigid = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        // Interaction();
        getInput();
        Swap();
    }

    void getInput()
    {

        oneDown = Input.GetKeyDown(KeyCode.Alpha1);
        twoDown = Input.GetKeyDown(KeyCode.Alpha2);
        threeDown = Input.GetKeyDown(KeyCode.Alpha3);
    }

    void Swap()
    {
        if (oneDown)
        {
            SwapServerRpc(0);
            localSwap(0);
        }
        if (twoDown)
        {
            SwapServerRpc(1);
            localSwap(1);
        }
        if (threeDown)
        {
            SwapServerRpc(2);
            localSwap(2);
        }
    }

    void localSwap(int weaponNumber)
    {
        animator.SetTrigger("doSwap");
        hasWeapon = weaponNumber;
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        weapons[hasWeapon].GetComponent<MWeapon>().gameObject.SetActive(true);
        equipWeapon = weapons[hasWeapon].GetComponent<MWeapon>();
    }

    [ServerRpc]
    void SwapServerRpc(int weaponNumber)
    {
        SwapClientRpc(weaponNumber);
    }

    [ClientRpc]
    void SwapClientRpc(int weaponNumber)
    {
        if (!IsOwner) localSwap(weaponNumber);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        break;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                bool isBossAttack = other.name == "BossMeleeArea";

                StartCoroutine(OnDamage(isBossAttack));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.GetComponent<Bullet>().OnHit();

            }
        }

    }
    IEnumerator OnDamage(bool isBossAttack)
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }
        if (isBossAttack)
        {
            //find better logic
            controller.Move(impact * 10 * Time.deltaTime);

        }

        yield return new WaitForSeconds(1f);
        if (isBossAttack)
            rigid.velocity = Vector3.zero;
        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

    }


}
