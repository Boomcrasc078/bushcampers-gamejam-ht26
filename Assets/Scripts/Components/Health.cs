using UnityEngine;
using UnityEngine.Video;

public class Health : MonoBehaviour {
    [SerializeField]private GameObject mainGameObject;
    private float healthAmount = 10f;
    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        if (healthAmount <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(mainGameObject);
    }

    public void Initialize(float maxHealth)
    {
        healthAmount = maxHealth;
    }
}