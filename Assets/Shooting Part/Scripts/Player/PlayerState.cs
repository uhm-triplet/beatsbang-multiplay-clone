using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public GameObject[] grenades;
    public int hasGrenades;

    public int health;


    public int maxHealth;
    public int maxHasGrenades;

    bool isDamage;

    GameObject nearObject;
    public Weapon equipWeapon1;
    public Weapon equipWeapon2;
    Animator animator;
    MeshRenderer[] meshs;
    Rigidbody rigid;

    Vector3 impact = Vector3.zero;
    private CharacterController controller;

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
        // Swap();
    }



    // void Swap()
    // {
    //     if ((hasWeapon == 0 || hasWeapon == 1 || hasWeapon == 2) && swapped)
    //     {
    //         if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);
    //         equipWeapon = weapons[hasWeapon].GetComponent<Weapon>();
    //         equipWeapon.gameObject.SetActive(true);

    //         animator.SetTrigger("doSwap");
    //         swapped = false;
    //     }
    // }

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
                Destroy(other.gameObject);
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
