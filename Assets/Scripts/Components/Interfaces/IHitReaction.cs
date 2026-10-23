using UnityEngine;

public interface IHitReaction {
    public void ReactToHit(Vector2 direction, float multiplier = 1f);
}