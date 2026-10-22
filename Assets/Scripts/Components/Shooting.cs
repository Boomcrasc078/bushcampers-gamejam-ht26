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

    private float bulletDamage = 1f;
    private bool isPlayerWeapon;

    private PiercingUpgrade piercingUpgrade;
    private KnockbackUpgrade knockbackUpgrade;


    void Awake()
    {
        shootingInterface = GetComponent<IShooting>();

        isPlayerWeapon = GetComponent<PlayerInput>() != null;

        if (isPlayerWeapon && ShopUpgradeManager.instance != null)
        {
            shootingRateUpgrade = ShopUpgradeManager.instance.GetUpgrade<ShootingRateUpgrade>();

            piercingUpgrade = ShopUpgradeManager.instance.GetUpgrade<PiercingUpgrade>();

            knockbackUpgrade = ShopUpgradeManager.instance.GetUpgrade<KnockbackUpgrade>();
        }
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

                float fireRate = shootingType.firePerSecond;

                if (shootingRateUpgrade != null)
                {
                    fireRate *= shootingRateUpgrade.GetShootingRateUpgrade();
                }

                yield return new WaitForSeconds(
                    1f / Mathf.Max(0.01f, fireRate)
                );
            }

            yield return null;
        }
    }

    private void Shoot()
    {

        if (AudioManager.instance != null && shootSound != null)
        {
            AudioManager.instance.PlaySound(shootSound, gameObject);
        }
        var bullets = shootingType.bulletsPerShot;
        for (int i = 0; i < bullets; i++)
        {
            var spread = shootingType.bulletSpreadDegrees;
            var spreadAngle = Random.Range(-spread / 2f, spread / 2f);
            var bulletRotation = rotationTransform.rotation * Quaternion.Euler(0f, 0f, spreadAngle);
            var bullet = GameObject.Instantiate(bulletPrefab, firePositionTransform.position, bulletRotation);


            string bulletLayerName = isPlayerWeapon ? "PlayerBullet" : "EnemyBullet";

            int bulletLayer = LayerMask.NameToLayer(bulletLayerName);

            if (bulletLayer >= 0)
            {
                bullet.layer = bulletLayer;
            }
            else
            {
                Debug.LogError($"Missing layer: {bulletLayerName}");
            }



            Bullet bulletComponent = bullet.GetComponent<Bullet>();

            if (bulletComponent != null)
            {
                int additionalPierces = piercingUpgrade != null ? piercingUpgrade.GetAdditionalPierces() : 0;
                float knockbackMultiplier = knockbackUpgrade != null ? knockbackUpgrade.GetKnockbackMultiplier() : 1f;

                bulletComponent.Initialize(bulletDamage, additionalPierces, knockbackMultiplier);
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
