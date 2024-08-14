using System;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Wall")) Destroy(this.gameObject);
        if(!other.gameObject.CompareTag("Player")) return;

        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Hit();
        }
        Destroy(this.gameObject);
    }
}
