using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
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
            Destroy(gameObject);
        }

    }
}
