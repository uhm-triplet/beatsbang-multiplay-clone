using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

public class PlayerItem : NetworkBehaviour
{
    public GameObject[] weapons;
    public int hasWeapon = 2;
    // public NetworkVariable<int> hasWeapon = new NetworkVariable<int>(2, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> swapped = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);

    public int ammo;
    public NetworkVariable<int> health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int hasGrenades;
    public GameObject[] grenades;

    public int kill;
    public int death;

    public int maxAmmo;
    public int maxHealth;
    public int maxHasGrenades;


    public Weapon equipWeapon;
    Animator animator;

    [SerializeField] GameObject UI;


    bool sDown;
    bool oneDown;
    bool twoDown;
    bool threeDown;

    [HideInInspector] public bool isDead = false;
    MeshRenderer[] meshs;


    public override void OnNetworkSpawn()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        if (!IsOwner) return;
        UI.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        getInput();
        Swap();
        if (health.Value <= 0 && !isDead)
        {
            animator.SetTrigger("doDie");
            isDead = true;
            StartCoroutine(DoRevive());
        }
    }

    void getInput()
    {
        sDown = Input.GetKeyDown(KeyCode.Z);
        oneDown = Input.GetKeyDown(KeyCode.Alpha1);
        twoDown = Input.GetKeyDown(KeyCode.Alpha2);
        threeDown = Input.GetKeyDown(KeyCode.Alpha3);
    }
    // void Swap()
    // {
    //     if (hasWeapon != -1 && swapped)
    //     {
    //         animator.SetTrigger("doSwap");
    //         SwapServerRpc();
    //         if (!IsServer)
    //         {
    //             if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
    //             equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
    //             equipWeapon.gameObject.SetActive(true);
    //             swapped = false;
    //         }
    //     }
    // }



    void Swap()
    {
        if (oneDown)
        {
            animator.SetTrigger("doSwap");
            UpdateSwapServerRpc(0);
            localSwap(0);
        }
        if (twoDown)
        {
            animator.SetTrigger("doSwap");
            UpdateSwapServerRpc(1);
            localSwap(1);

        }
        if (threeDown)
        {
            animator.SetTrigger("doSwap");
            UpdateSwapServerRpc(2);
            localSwap(2);
        }

    }


    void localSwap(int weaponNumber)
    {
        hasWeapon = weaponNumber;
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        weapons[hasWeapon].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateSwapServerRpc(int weaponNumber)
    {
        NotifyClientSwapClientRpc(weaponNumber);
    }

    [ClientRpc]
    void NotifyClientSwapClientRpc(int weaponNumber)
    {
        if (IsOwner) return;
        localSwap(weaponNumber);
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Item item = other.GetComponent<Item>();
            hasWeapon = item.value;
            swapped.Value = !swapped.Value;
            // Destroy(other.gameObject);
        }

        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Heart:
                    health.Value += item.value;
                    if (health.Value > maxHealth)
                        health.Value = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        break;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (IsOwner)
            {
                health.Value -= bullet.damage;
            }
            else
            {
                health.Value = health.Value;
            }
            OnDamageClientRpc();
            LocalOnDamage();
            // StartCoroutine(OnDamage());
        }
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            health.Value -= weapon.damage;
            StartCoroutine(OnDamage());
        }
    }

    void LocalOnDamage()
    {
        StartCoroutine(OnDamage());
    }

    [ServerRpc]
    void OnDamageServerRpc()
    {
        OnDamageClientRpc();
    }

    [ClientRpc]
    void OnDamageClientRpc()
    {
        if (IsOwner) return;
        LocalOnDamage();
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        health.Value = 0;
        StartCoroutine(OnDamage());
    }
    IEnumerator OnDamage()
    {
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

    IEnumerator DoRevive()
    {
        yield return new WaitForSeconds(5);
        isDead = false;
        health.Value = 100;
    }


}
