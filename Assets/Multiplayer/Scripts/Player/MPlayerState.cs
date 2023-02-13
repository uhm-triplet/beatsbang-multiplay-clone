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
    public int health = 100;
    // public NetworkVariable<int> health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
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

    [SerializeField] GameObject UI;
    bool oneDown;
    bool twoDown;
    bool threeDown;
    [HideInInspector] public bool isDead = false;

    public override void OnNetworkSpawn()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (!IsOwner) return;
        UI.SetActive(true);
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
        Die();

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

        // if (other.tag == "Item")
        // {
        //     Item item = other.GetComponent<Item>();
        //     switch (item.type)
        //     {
        //         case Item.Type.Grenade:
        //             if (hasGrenades == maxHasGrenades)
        //                 break;
        //             grenades[hasGrenades].SetActive(true);
        //             hasGrenades += item.value;
        //             break;
        //     }
        //     Destroy(other.gameObject);
        // }


        if (other.tag == "Bullet")
        {
            OnDamageClientRpc();
            LocalOnDamage();
        }
        if (other.tag == "Melee")
        {

            health -= 1;
            OnDamageClientRpc();
            LocalOnDamage();
        }

    }

    public void LocalOnDamage()
    {
        StartCoroutine(OnDamage());
    }

    [ServerRpc]
    void OnDamageServerRpc()
    {
        OnDamageClientRpc();
    }

    [ClientRpc]
    public void OnDamageClientRpc()
    {
        if (IsOwner) return;
        LocalOnDamage();
    }


    public void HitByGrenade()
    {
        health = 0;
        StartCoroutine(OnDamage());
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }


        yield return new WaitForSeconds(0.5f);

        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

    }

    void Die()
    {
        if (health <= 0 && !isDead)
        {
            DieServerRpc();
            LocalDie();
        }
    }
    public void LocalDie()
    {
        animator.SetTrigger("doDie");
        isDead = true;
        if (gameObject.tag == "Player")
        {
            ScoreManager.Instance.playerADeath++;
            ScoreManager.Instance.playerBKill++;
        }
        else
        {
            ScoreManager.Instance.playerBDeath++;
            ScoreManager.Instance.playerAKill++;
        }
        StartCoroutine(DoRevive());

    }

    [ServerRpc]
    void DieServerRpc()
    {
        DieClientRpc();
    }

    [ClientRpc]
    public void DieClientRpc()
    {
        if (IsOwner) return;
        LocalDie();
    }


    IEnumerator DoRevive()
    {
        yield return new WaitForSeconds(5);
        isDead = false;
        health = 100;
    }


}
