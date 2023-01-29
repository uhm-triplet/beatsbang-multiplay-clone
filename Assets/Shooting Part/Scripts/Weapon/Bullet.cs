using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public int damage;
    [SerializeField] float bulletVelocity = 300;
    private Rigidbody rigid;
    public override void OnNetworkSpawn()
    {
        Rigidbody bulletRigid = GetComponent<Rigidbody>();
        bulletRigid.AddForce(transform.forward * bulletVelocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        //사라지는 기준 정확하게 바꾸기
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

    }
}
