using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGrenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 25, Vector3.up, 0, LayerMask.GetMask("Player"));
        RaycastHit[] rayHits2 = Physics.SphereCastAll(transform.position, 25, Vector3.up, 0, LayerMask.GetMask("Player2"));
        foreach (RaycastHit hit in rayHits)
        {
            hit.transform.GetComponent<MPlayerState>().HitByGrenade();
        }
        foreach (RaycastHit hit in rayHits2)
        {
            hit.transform.GetComponent<MPlayerState>().HitByGrenade();
        }
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
