using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> healthPoints = new NetworkVariable<int>(10);

    public static event System.Action<int> ChangedHealthPointsEvent;
    public static event System.Action GameOverEvent;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            healthPoints.OnValueChanged += HealthPointsChangedEvent;
        }
    }

    public void Heal()
    {
        healthPoints.Value += 2;
        HealthPointsChanged();
    }

    public void Hit()
    {
        if (IsOwner) HitServerRPC();
    }

    [ServerRpc]
    private void HitServerRPC(ServerRpcParams rpcParams = default)
    {
        if (healthPoints.Value > 1)
        {
            healthPoints.Value -= 1;
            HealthPointsChanged();
            
            HealthPointsChangedClientRpc(healthPoints.Value);
        }
        else
        {
            PlayerGameOverClientRpc();
        }
    }

    [ClientRpc]
    private void PlayerGameOverClientRpc(ClientRpcParams rpcParams = default)
    {
        if (IsOwner)
        {
            Debug.Log("Player died");
            GameOverEvent?.Invoke();
            NetworkManager.Singleton.Shutdown();   
        }
    }
    
    [ClientRpc]
    private void HealthPointsChangedClientRpc(int newHealth, ClientRpcParams rpcParams = default)
    {
        if (IsOwner)
        {
            ChangedHealthPointsEvent?.Invoke(newHealth);
        }
    }

    private void HealthPointsChangedEvent(int previousValue, int newValue)
    {
        HealthPointsChanged();
    }

    private void HealthPointsChanged()
    {
        if (!IsOwner) return;
        ChangedHealthPointsEvent?.Invoke(healthPoints.Value);
    }
}
