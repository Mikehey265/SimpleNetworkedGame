using Unity.Netcode;
using UnityEngine;

public class PlayerAmmo : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float bulletSpeed;
    public NetworkVariable<int> ammoCount = new(10);

    public static event System.Action<int> ChangedAmmoCountEvent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            ammoCount.OnValueChanged += AmmoCountChangedEvent;
        }
    }

    public void AddAmmo()
    {
        ammoCount.Value += 10;
        AmmoCountChanged();
    }

    public void Shoot()
    {
        if (IsOwner)
        {
            ShootServerRpc(bulletSpeed);
        }
    }
    
    [ServerRpc]
    private void ShootServerRpc(float speed)
    {
        if (ammoCount.Value > 0)
        {
            ammoCount.Value -= 1;
            Vector2 velocity = gunTip.up * speed;
            InstantiateBulletClientRpc(velocity);   
        }
    }

    [ClientRpc]
    private void InstantiateBulletClientRpc(Vector2 bulletVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        AmmoCountChanged();
    }
    
    private void AmmoCountChangedEvent(int previousValue, int newValue)
    {
        AmmoCountChanged();
    }
    
    private void AmmoCountChanged()
    {
        if(!IsOwner) return;
        ChangedAmmoCountEvent?.Invoke(ammoCount.Value);
    }
}
