using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

public class PlayerItem : NetworkBehaviour
{
    public GameObject[] weapons;
    // public int hasWeapon.Value = -1;
    public NetworkVariable<int> hasWeapon = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    bool swapped;

    public int ammo;
    public int health;
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
    int rnd = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        UI.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        equipWeapon = weapons[hasWeapon.Value].GetComponent<Weapon>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        getInput();

        if (sDown)
        {

            hasWeapon.Value = new Random().Next(0, 3);
        }
        Swap();


    }

    void getInput()
    {
        sDown = Input.GetKeyDown(KeyCode.Z);

    }
    // void Swap()
    // {
    //     if (hasWeapon.Value != -1 && swapped)
    //     {
    //         animator.SetTrigger("doSwap");
    //         SwapServerRpc();
    //         if (!IsServer)
    //         {
    //             if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
    //             equipWeapon = weapons[hasWeapon.Value].GetComponent<Weapon>();
    //             equipWeapon.gameObject.SetActive(true);
    //             swapped = false;
    //         }
    //     }
    // }


    void localSwap()
    {
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        weapons[hasWeapon.Value].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[hasWeapon.Value].GetComponent<Weapon>();
    }
    void Swap()
    {

        if (sDown)
        {
            animator.SetTrigger("doSwap");
            SwapServerRpc();
            if (IsServer)
            {
                SwapClientRpc();
            }
            if (!IsServer)
            {
                localSwap();
            }

        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SwapServerRpc()
    {
        localSwap();
    }

    [ClientRpc]
    void SwapClientRpc()
    {
        localSwap();
    }



    // [ServerRpc(RequireOwnership = false)]
    // private void SwapServerRpc()
    // {

    //     if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
    //     equipWeapon = weapons[hasWeapon.Value].GetComponent<Weapon>();
    //     equipWeapon.gameObject.SetActive(true);

    //     swapped = false;

    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Item item = other.GetComponent<Item>();
            hasWeapon.Value = item.value;
            swapped = true;
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
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
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

    }
}
