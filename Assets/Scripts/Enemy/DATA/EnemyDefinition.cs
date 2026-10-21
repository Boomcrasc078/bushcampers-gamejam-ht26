using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public GameObject prefab;

    [Header("Stats")]
    public float maxHealth = 10f;
    public float moveSpeed = 3f;
    public float damage = 1f;

    [Header("Combat")]
    public float attackRange = 1.5f;

    [Header("Melee Attack")]
    public float attackWindup = 0.45f;
    public float attackRecovery = 0.6f;
    public float attackCooldown = 0.3f;
    public float knockbackStrength = 0f;


    [Header("Spawning")]
    public float spawnWeight = 1f;
}
