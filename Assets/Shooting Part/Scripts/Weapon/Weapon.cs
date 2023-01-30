using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Weapon : NetworkBehaviour
{
    public enum Type { Melee, Range }
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public GameObject bullet;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;






    // private void Shot()
    // {
    //     Debug.Log("Shooting");

    //     if (!IsServer)
    //         ShotServerRpc();
    //     else
    //     {
    //         bulletPos.LookAt(playerAim.aimPos);
    //         GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
    //         spawnedBullets.Add(instantBullet);
    //         instantBullet.GetComponent<Bullet>().parent = this;
    //         instantBullet.GetComponent<NetworkObject>().Spawn();
    //     }

    // }

    // [ServerRpc]
    // private void ShotServerRpc()
    // {
    //     // animator.SetTrigger("doShot");
    //     bulletPos.LookAt(playerAim.aimPos);
    //     GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
    //     spawnedBullets.Add(instantBullet);
    //     instantBullet.GetComponent<Bullet>().parent = this;
    //     instantBullet.GetComponent<NetworkObject>().Spawn();
    // }

    // [ClientRpc]
    // private void ShotClientRpc()
    // {
    //     animator.SetTrigger("doShot");
    // }

    // [ServerRpc(RequireOwnership = false)]
    // public void DestroyBulletServerRpc()
    // {
    //     GameObject toDestroy = spawnedBullets[0];
    //     toDestroy.GetComponent<NetworkObject>().Despawn();
    //     spawnedBullets.Remove(toDestroy);
    //     Destroy(toDestroy);
    // }
    // Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
    // bulletRigid.AddForce(bulletPos.forward * bulletVelocity, ForceMode.Impulse);
    // Rigidbody bulletRigid = syncBullet.GetComponent<Rigidbody>();
    // bulletRigid.AddForce(bulletPos.forward * bulletVelocity, ForceMode.Impulse);


    // yield return null;
    // GameObject instantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
    // Rigidbody bulletCaseRigid = instantBulletCase.GetComponent<Rigidbody>();
    // Vector3 caseVec = bulletCasePos.forward * Random.Range(-4, -3) + Vector3.up * Random.Range(2, 3);
    // bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
    // bulletCaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);



}
