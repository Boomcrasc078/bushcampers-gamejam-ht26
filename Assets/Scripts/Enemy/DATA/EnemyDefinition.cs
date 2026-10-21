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

    [Header("Spawning")]
    public float spawnWeight = 1f;
}
