using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerItem : NetworkBehaviour
{
    public GameObject[] weapons;
    public int hasWeapon = -1;
    // public NetworkVariable<int> hasWeapon = new NetworkVariable<int>(-1);
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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        UI.SetActive(true);
        animator = GetComponentInChildren<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        getInput();
        Swap();


    }

    void getInput()
    {
        sDown = Input.GetKeyDown(KeyCode.Z);

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
        if (sDown)
        {
            animator.SetTrigger("doSwap");
            SwapServerRpc();
            if (!IsServer)
            {
                hasWeapon = 2;
                weapons[2].GetComponent<Weapon>().gameObject.SetActive(true);
                equipWeapon = weapons[2].GetComponent<Weapon>();
            }
            if (IsServer)
            {
                SwapClientRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SwapServerRpc()
    {
        hasWeapon = 2;
        weapons[2].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[2].GetComponent<Weapon>();
    }

    [ClientRpc]
    void SwapClientRpc()
    {
        hasWeapon = 2;
        weapons[2].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[2].GetComponent<Weapon>();
    }



    // [ServerRpc(RequireOwnership = false)]
    // private void SwapServerRpc()
    // {

    //     if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
    //     equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
    //     equipWeapon.gameObject.SetActive(true);

    //     swapped = false;

    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Item item = other.GetComponent<Item>();
            hasWeapon = item.value;
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
