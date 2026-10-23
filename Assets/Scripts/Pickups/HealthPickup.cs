
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 2f;
    [SerializeField] private Sound pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySound(pickupSound, AudioManager.instance.gameObject);
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
