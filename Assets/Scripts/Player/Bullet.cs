using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float damage = 1f;

    public void Initialize(float damage)
    {
        this.damage = damage;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Health hitHealth = collision.gameObject.GetComponent<Health>();

        if (hitHealth != null)
        {
            hitHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void Awake()
    {
        Invoke(nameof(DestroyGameobject), 5f);
    }

    void DestroyGameobject()
    {
        Destroy(gameObject);
    }
}