using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBullet : MonoBehaviour
{
    public int damage;

    public GameObject meshObj;
    public GameObject effectObj;

    public Rigidbody rigid;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 1);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            // MPlayerState playerState = other.gameObject.GetComponent<MPlayerState>();
            MPlayerState playerState = other.gameObject.GetComponent<MPlayerState>();
            playerState.health -= damage;
            // playerState.LocalOnDamage();
        }
    }

}
