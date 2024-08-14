using Unity.Netcode;
using UnityEngine;

public class HealthPickup : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        if(!NetworkManager.Singleton.IsServer) return;

        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Heal();
        }
        
        DespawnRespawn();
    }

    private void DespawnRespawn()
    {
        var spawner = FindObjectOfType<PickupSpawnManager>();
        if (spawner != null)
        {
            spawner.RespawnPickup(spawner.healthPickupPrefab);
        }
        NetworkObject.Despawn();
    }
}
