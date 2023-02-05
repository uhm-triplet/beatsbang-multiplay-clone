using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    public int damage;
    public float rate;


    [SerializeField] Transform bulletPos;
    public GameObject bullet;
    [SerializeField] float bulletVelocity;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    PlayerAim playerAim;

    void Start()
    {
        playerAim = GetComponentInParent<PlayerAim>();
    }


    public void Use()
    {

        StartCoroutine("Shot");

    }



    IEnumerator Shot()
    {

        bulletPos.LookAt(playerAim.aimPos);
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.AddForce(bulletPos.forward * bulletVelocity, ForceMode.Impulse);

        yield return null;
        GameObject instantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody bulletCaseRigid = instantBulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-4, -3) + Vector3.up * Random.Range(2, 3);
        bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
        bulletCaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);


    }
}
