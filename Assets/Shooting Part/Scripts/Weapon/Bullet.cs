using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public PlayerWeapon parent;
    public int damage;
    [SerializeField] public float bulletVelocity;
    private Rigidbody rigid;
    void Start()
    {

        Rigidbody bulletRigid = GetComponent<Rigidbody>();
        bulletRigid.AddForce(transform.forward * bulletVelocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        //사라지는 기준 정확하게 바꾸기
        if (!IsOwner) return;
        // Destroy(gameObject);
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Floor")
        {
            parent.DestroyBulletServerRpc();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        // Destroy(gameObject);
        parent.DestroyBulletServerRpc();

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Floor")
        {

        }

        if (other.gameObject.tag == "Player")
        {
            PlayerBulletHitServerRpc(damage);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    void PlayerBulletHitServerRpc(int damage)
    {
        if (NetworkManager.ConnectedClients.ContainsKey(OwnerClientId))
        {
            var client = NetworkManager.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<PlayerItem>();
            client.health.Value -= damage;
        }
        if (IsServer)
        {
            NotifyClientBulletHitClientRpc(OwnerClientId);
        }
    }

    [ClientRpc]
    void NotifyClientBulletHitClientRpc(ulong clientId)
    {
        if (IsOwner) return;
        Debug.Log(clientId + " Is Hit");
    }
}
