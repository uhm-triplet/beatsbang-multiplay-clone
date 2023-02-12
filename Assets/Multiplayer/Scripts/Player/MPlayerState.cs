using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerState : MonoBehaviour
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

    void Awake()
    {
        impact.z = -50;
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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
            SwapLogic(0);
        }
        if (twoDown)
        {
            SwapLogic(1);

        }
        if (threeDown)
        {
            SwapLogic(2);

        }
    }

    void SwapLogic(int weaponNo)
    {
        hasWeapon = weaponNo;
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
        equipWeapon = weapons[hasWeapon].GetComponent<MWeapon>();
        equipWeapon.gameObject.SetActive(true);

        animator.SetTrigger("doSwap");
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
