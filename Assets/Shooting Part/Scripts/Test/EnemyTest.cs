using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class EnemyTest : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    public GameObject bullet;
    public Transform bulletPos;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }
    void Start()
    {
        StartCoroutine("EnemyShootStart");

    }

    IEnumerator EnemyShootStart()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine("EnemyShoot");
    }

    IEnumerator EnemyShoot()
    {
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        instantBullet.GetComponent<NetworkObject>().Spawn();
        yield return new WaitForSeconds(1);
        Destroy(instantBullet);
        StartCoroutine("EnemyShoot");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            currentHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
            Debug.Log("근접");
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            currentHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
            Debug.Log("원거리");
        }

    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        currentHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec));
    }
    // Update is called once per frame
    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (currentHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 11;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4);
        }

    }
}
