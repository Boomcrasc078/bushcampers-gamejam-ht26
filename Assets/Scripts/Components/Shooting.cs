using UnityEngine;
[RequireComponent(typeof(IShooting))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private Transform firePositionTransform;
    IShooting shootingInterface;

    void Awake()
    {
        shootingInterface = GetComponent<IShooting>();
    }

    void Start()
    {
        shootingInterface.Shoot += Shoot;
    }

    private void Shoot()
    {
        var bullet = GameObject.Instantiate(bulletPrefab, firePositionTransform.position, rotationTransform.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        var bulletCollider = bullet.GetComponent<Collider2D>();
        bulletRigidbody.AddRelativeForce(Vector2.up * 400f);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
