using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [Tooltip("Enemies that will be spawned")]
    public List<GameObject> Enemies;

    [Header("Spawning Information")]
    [Tooltip("Positions that enemies will spawn from")]
    public List<Transform> spawnPos;
    [SerializeField, Tooltip("How long it takes for an enemy to spawn")]
    private float spawnCooldown;
    [SerializeField, Tooltip("How many enemies will spawn")]
    private int enemyAmount;

    public void Start()
    {
        GameObject enemy = Instantiate(Enemies[0], spawnPos[0]);
        enemy.SetActive(true);
    }

    public GameObject SpawnEnemy()
    {
        int eIndex = UnityEngine.Random.Range(0, Enemies.Count);
        int sIndex = UnityEngine.Random.Range(0, Enemies.Count);
        GameObject enemy = Instantiate(Enemies[eIndex], spawnPos[sIndex]);
        GetComponent<EnemyController>().enemies.Add(enemy);
        return enemy;
    }
}
