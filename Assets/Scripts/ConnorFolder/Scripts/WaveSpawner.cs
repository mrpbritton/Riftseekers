using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : Singleton<WaveSpawner> {
    [Header("Spawning Information")]
    [Tooltip("How many enemies will spawn")]
    [SerializeField] int monstersPerWave = 50;
    [SerializeField] float timeBtwWaves = 10f;
    [Tooltip("How long it takes for an enemy to spawn")]
    [SerializeField] float minSpawnTime = .15f, maxSpawntime = .35f;
    bool waveDone = false;

    [Tooltip("Enemies that will be spawned")]
    public List<GameObject> enemies = new List<GameObject>();

    [Tooltip("Positions that enemies will spawn from")]
    public List<Transform> spawnPos = new List<Transform>();

    EnemyController ec;

    private void Start() {
        EnemyController.levelComplete += triggerEndOfWave;
        ec = FindObjectOfType<EnemyController>();
        StartCoroutine(wave());
    }

    private void OnDisable() {
        EnemyController.levelComplete -= triggerEndOfWave;
    }

    IEnumerator wave() {
        while(true) {
            waveDone = false;
            for(int i = 0; i < monstersPerWave; i++) {
                ec.enemies.Add(Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos[Random.Range(0, spawnPos.Count)]));
                yield return new WaitForSeconds(Random.Range(.15f, .35f));
            }
            while(!waveDone)
                yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(timeBtwWaves);    //  TIME BTW WAVES
        }
    }

    void triggerEndOfWave() {
        waveDone = true;
    }
}