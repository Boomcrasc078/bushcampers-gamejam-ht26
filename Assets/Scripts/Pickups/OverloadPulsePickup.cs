
using UnityEngine;

public class OverloadPulsePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
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
