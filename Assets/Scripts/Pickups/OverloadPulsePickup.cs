
using UnityEngine;

public class OverloadPulsePickup : MonoBehaviour
{
    [SerializeField] private Sound pickupSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySound(pickupSound, AudioManager.instance.gameObject);

        OverloadPulseAbility ability =
            other.GetComponentInParent<OverloadPulseAbility>();

        if (ability == null)
            return;

        if (ability.TryCollect())
        {
            Destroy(gameObject);
        }
    }
}
