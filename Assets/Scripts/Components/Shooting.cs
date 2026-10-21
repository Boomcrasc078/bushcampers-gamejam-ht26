using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(IShooting))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private ShootingType shootingType;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private Transform firePositionTransform;
    IShooting shootingInterface;
    private ShootingRateUpgrade shootingRateUpgrade;

    private float bulletDamage = 1f;


    void Awake()
    {
        shootingInterface = GetComponent<IShooting>();
        shootingRateUpgrade = GetComponent<ShootingRateUpgrade>();
    }

    void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            while (shootingInterface.IsShooting())
            {
                Shoot();
                if (ShopUpgradeManager.instance.ActivatedUpgrades.Contains(shootingRateUpgrade))
                {
                    yield return new WaitForSeconds(1 / shootingType.firePerSecond * shootingRateUpgrade.GetShootingRateUpgrade());
                }
                else
                {
                    yield return new WaitForSeconds(1 / shootingType.firePerSecond);
                }
            }
            yield return null;
        }
    }
    private void Shoot()
    {
        var bullets = shootingType.bulletsPerShot;
        for (int i = 0; i < bullets; i++)
        {
            var spread = shootingType.bulletSpreadDegrees;
            var spreadAngle = Random.Range(-spread / 2f, spread / 2f);
            var bulletRotation = rotationTransform.rotation * Quaternion.Euler(0f, 0f, spreadAngle);
            var bullet = GameObject.Instantiate(bulletPrefab, firePositionTransform.position, bulletRotation);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();

            if(bulletComponent != null)
            {
                bulletComponent.Initialize(bulletDamage);
            }

            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

            bulletRigidbody.AddRelativeForce(Vector2.up * shootingType.bulletSpeed);
        }
    }

    public void Initialize(float damage)
    {
        bulletDamage = damage;
    }


}
