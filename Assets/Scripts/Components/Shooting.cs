using System.Collections;
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
    private ProjectileVelocityUpgrade projectileVelocityUpgrade;
    private ProjectileDamageUpgrade projectileDamageUpgrade;
    private MultishotUpgrade multishotUpgrade;


    void Awake()
    {
        shootingInterface = GetComponent<IShooting>();

        isPlayerWeapon = GetComponent<PlayerInput>() != null;

        if (isPlayerWeapon && ShopUpgradeManager.instance != null)
        {
            shootingRateUpgrade = ShopUpgradeManager.instance.GetUpgrade<ShootingRateUpgrade>();

            piercingUpgrade = ShopUpgradeManager.instance.GetUpgrade<PiercingUpgrade>();

            knockbackUpgrade = ShopUpgradeManager.instance.GetUpgrade<KnockbackUpgrade>();

            projectileVelocityUpgrade = ShopUpgradeManager.instance.GetUpgrade<ProjectileVelocityUpgrade>();

            projectileDamageUpgrade = ShopUpgradeManager.instance.GetUpgrade<ProjectileDamageUpgrade>();

            multishotUpgrade = ShopUpgradeManager.instance.GetUpgrade<MultishotUpgrade>();
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
        int extraBullets = multishotUpgrade != null ? multishotUpgrade.GetAdditionalProjectiles() : 0;
        int bullets = Mathf.Max(1, shootingType.bulletsPerShot + extraBullets);

        for (int i = 0; i < bullets; i++)
        {
            float centeredIndex = i - (bullets - 1) / 2f;
            float multishotAngle = isPlayerWeapon ? centeredIndex * shootingType.multishotAngleStep : 0f;

            float spread = shootingType.bulletSpreadDegrees;
            float randomSpread = Random.Range(-spread / 2f, spread / 2f);

            Quaternion bulletRotation = rotationTransform.rotation * Quaternion.Euler(0f, 0f, multishotAngle + randomSpread);
            GameObject bullet = Instantiate(bulletPrefab, firePositionTransform.position, bulletRotation);


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

                float damageMultiplier = projectileDamageUpgrade != null ? projectileDamageUpgrade.GetDamageMultiplier() : 1f;
                bulletComponent.Initialize(bulletDamage * damageMultiplier, additionalPierces, knockbackMultiplier);
            }

            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

            float speedMultiplier = projectileVelocityUpgrade != null ? projectileVelocityUpgrade.GetSpeedMultiplier() : 1f;
            float projectileSpeed = shootingType.bulletSpeed * speedMultiplier;

            bulletRigidbody.linearVelocity = (Vector2)bullet.transform.up * projectileSpeed;
        }
    }

    public void Initialize(float damage)
    {
        bulletDamage = damage;
    }


}
