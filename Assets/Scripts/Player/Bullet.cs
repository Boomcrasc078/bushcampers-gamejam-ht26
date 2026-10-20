using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Health hitHealth = collision.gameObject.GetComponent<Health>();

        if (hitHealth != null)
        {
            hitHealth.TakeDamage(1f);
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