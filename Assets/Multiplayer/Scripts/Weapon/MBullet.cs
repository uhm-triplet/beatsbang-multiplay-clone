using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
    public GameObject meshObj;
    public GameObject effectObj;

    public Rigidbody rigid;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision other)
    {
        //사라지는 기준 정확하게 바꾸기
        if (!isRock && other.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 1);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
        {
            if (gameObject.tag == "EnemyBullet")
                OnHit();
            else
                Destroy(gameObject);
        }

    }
    public void OnHit()
    {
        StartCoroutine(OnHitCoroutine());
    }
    IEnumerator OnHitCoroutine()
    {
        rigid.velocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
