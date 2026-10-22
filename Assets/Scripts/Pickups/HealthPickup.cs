
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInput player = other.GetComponentInParent<PlayerInput>();

        if (player == null)
            return;

        Health playerHealth = player.GetComponentInChildren<Health>();

        if (playerHealth != null && playerHealth.Heal(healAmount))
        {
            Destroy(gameObject);
        }
    }
}
