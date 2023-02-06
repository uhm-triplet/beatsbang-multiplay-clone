using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossMissile : Bullet
{
    public Transform target;
    // Rigidbody rigid;

    NavMeshAgent nav;
    // Start is called before the first frame update
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        // rigid = GetComponent<Rigidbody>();
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(4f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        meshObj.SetActive(false);
        effectObj.SetActive(true);


        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


}
