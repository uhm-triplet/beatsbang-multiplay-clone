using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItem : MonoBehaviour
{
    public GameObject[] weapons;
    public int hasWeapon = 2;
    // public NetworkVariable<int> hasWeapon = new NetworkVariable<int>(2, NetworkVariableReadPermission.Everyone);
    // public NetworkVariable<bool> swapped = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);

    public int ammo;
    public int health;
    // public NetworkVariable<int> health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone);
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


    void Awake()
    {

        UI.SetActive(true);
        animator = GetComponentInChildren<Animator>();
        equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
        meshs = GetComponentsInChildren<MeshRenderer>();

    }
    // Update is called once per frame
    void Update()
    {

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


    void localSwap(int weaponNo)
    {
        hasWeapon = weaponNo;
        animator.SetTrigger("doSwap");
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        weapons[hasWeapon].GetComponent<Weapon>().gameObject.SetActive(true);
        equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
        swapped.Value = !swapped.Value;
    }
    void Swap()
    {
        if (oneDown)
        {
            localSwap(0);
            // SwapServerRpc();
        }
        if (twoDown)
        {
            localSwap(1);
            // SwapServerRpc();
        }
        if (threeDown)
        {
            localSwap(2);
            // SwapServerRpc();
        }
        // if (swapped.Value)
        // {

        // }
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

        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            health -= bullet.damage;
            StartCoroutine(OnDamage());
        }
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            health -= weapon.damage;
            StartCoroutine(OnDamage());
        }
    }


    public void HitByGrenade(Vector3 explosionPos)
    {
        health = 0;
        StartCoroutine(OnDamage());
    }
    IEnumerator OnDamage()
    {
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }
        if (health <= 0 && !isDead)
        {
            animator.SetTrigger("doDie");
            isDead = true;
            StartCoroutine(DoRevive());
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
        health = 100;
    }


}
