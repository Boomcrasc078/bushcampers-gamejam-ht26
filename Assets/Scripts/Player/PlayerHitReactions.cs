using UnityEngine;

public class PlayerHitReactions : MonoBehaviour, IHitReaction
{
    [Header("Particle Effects")]
    [SerializeField] GameObject bloodSplatPrefab;

    public void ReactToHit(Vector2 direction, float multiplier = 1)
    {
        Instantiate(bloodSplatPrefab, transform.position, new Quaternion());
    }
}