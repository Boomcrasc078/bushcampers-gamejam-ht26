
using UnityEngine;

[CreateAssetMenu(fileName = "ShootingType", menuName = "ShootingType", order = 0)]
public class ShootingType : ScriptableObject
{
    public float firePerSecond = 1f;
    public int bulletsPerShot = 1;

    [Header("Spread")]
    public float bulletSpreadDegrees = 0f;
    public float multishotAngleStep = 12f;

    public float bulletSpeed = 10f;
}
