using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupSpawnManager : NetworkBehaviour
{
    public GameObject ammoPickupPrefab;
    public GameObject healthPickupPrefab;
    [SerializeField] private int maxHealthPickups;
    [SerializeField] private int maxAmmoPickups;
    [SerializeField] private float respawnTime;

    private Vector2 screenBounds;
    private Camera mainCamera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            Initialize();   
        }
    }

    private void Initialize()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        SpawnPickups(maxHealthPickups, healthPickupPrefab);
        SpawnPickups(maxAmmoPickups, ammoPickupPrefab);
    }

    private void SpawnPickups(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition = GetRandomPosition();
            GameObject pickup = Instantiate(prefab, spawnPosition, Quaternion.identity);
            pickup.GetComponent<NetworkObject>().Spawn();
        }  
    }

    private Vector2 GetRandomPosition()
    {
        float x = Random.Range(-screenBounds.x + 1, screenBounds.x - 1 );
        float y = Random.Range(-screenBounds.y + 1, screenBounds.y - 1);
        return new Vector2(x, y);
    }

    public void RespawnPickup(GameObject pickupPrefab)
    {
        RespawnPickupCoroutine(pickupPrefab);
    }

    private void RespawnPickupCoroutine(GameObject pickupPrefab)
    {
        if(NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            Vector2 spawnPosition = GetRandomPosition();
            GameObject pickup = Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
            pickup.GetComponent<NetworkObject>().Spawn();   
        }
    }
}
