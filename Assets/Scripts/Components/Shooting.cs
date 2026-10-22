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
    [Header("Audio")]
    [SerializeField] private Sound shootSound;

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
        AudioManager.instance.PlaySound(shootSound, gameObject);
        var bullets = shootingType.bulletsPerShot;
        for (int i = 0; i < bullets; i++)
        {
            var spread = shootingType.bulletSpreadDegrees;
            var spreadAngle = Random.Range(-spread / 2f, spread / 2f);
            var bulletRotation = rotationTransform.rotation * Quaternion.Euler(0f, 0f, spreadAngle);
            var bullet = GameObject.Instantiate(bulletPrefab, firePositionTransform.position, bulletRotation);
            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

            bulletRigidbody.AddRelativeForce(Vector2.up * shootingType.bulletSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
