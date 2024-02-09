using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
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
    private int currentEnemyCount;
    private bool bHasSpawned;

    public void Start()
    {
        GameObject enemy = Instantiate(Enemies[0], spawnPos[0]);
        enemy.SetActive(true);
    }

    private void Update()
    {
        if (bHasSpawned || currentEnemyCount >= enemyAmount-1) return;
        bHasSpawned = true;
        currentEnemyCount++;
        int eIndex = UnityEngine.Random.Range(0, Enemies.Count);
        int sIndex = UnityEngine.Random.Range(0, Enemies.Count);
        StartCoroutine(SpawnEnemy(eIndex, sIndex));
    }

    private IEnumerator SpawnEnemy(int eIndex, int sIndex)
    {
        GameObject enemy = Instantiate(Enemies[eIndex], spawnPos[sIndex]);
        enemy.SetActive(true);
        yield return new WaitForSeconds(spawnCooldown);
        bHasSpawned = false;
    }
}
