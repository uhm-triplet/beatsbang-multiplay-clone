using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

public class PlayerItem : NetworkBehaviour
{
    public GameObject[] weapons;
    // public int hasWeapon = 2;
    public NetworkVariable<int> hasWeapon = new NetworkVariable<int>(2, NetworkVariableReadPermission.Everyone);
    public NetworkVariable<bool> swapped = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);

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
    bool oneDown;
    bool twoDown;
    bool threeDown;


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
        Swap();
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
        // animator.SetTrigger("doSwap");
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        weapons[hasWeapon.Value].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[hasWeapon.Value].GetComponent<Weapon>();
        swapped.Value = !swapped.Value;
    }
    void Swap()
    {
        if (oneDown)
        {
            UpdateSwapServerRpc(0);
            animator.SetTrigger("doSwap");
            SwapServerRpc();
        }
        if (twoDown)
        {
            UpdateSwapServerRpc(1);
            animator.SetTrigger("doSwap");
            SwapServerRpc();
        }
        if (threeDown)
        {
            UpdateSwapServerRpc(2);
            animator.SetTrigger("doSwap");
            SwapServerRpc();
        }
        // if (swapped.Value)
        // {

        // }
    }



    [ServerRpc(RequireOwnership = false)]
    void UpdateSwapServerRpc(int weaponNumber)
    {
        if (NetworkManager.ConnectedClients.ContainsKey(OwnerClientId))
        {
            var client = NetworkManager.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<PlayerItem>();
            client.hasWeapon.Value = weaponNumber;
            swapped.Value = !swapped.Value;
        }
    }

    [ClientRpc]
    void NotifyClientSwapClientRpc(int weaponNumber, ulong clientId)
    {
        if (IsOwner) return;

    }

    [ServerRpc(RequireOwnership = false)]
    void SwapServerRpc()
    {
        localSwap();
        if (IsServer)
        {
            SwapClientRpc();
        }
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
