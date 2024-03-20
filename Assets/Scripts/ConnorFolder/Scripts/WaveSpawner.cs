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
    [SerializeField] int enemyNumberIncrease = 10;
    bool waveDone = false;

    [Tooltip("Enemies that will be spawned")]
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    KdTree<Transform> spawnPoints = new KdTree<Transform>();

    EnemyController ec;
    Transform playerTrans;

    private void Start() {
        EnemyController.levelComplete += triggerEndOfWave;
        ec = FindObjectOfType<EnemyController>();
        for(int i = 0; i < transform.childCount; i++)
            spawnPoints.Add(transform.GetChild(i));
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        StartCoroutine(wave());
    }

    private void OnDisable() {
        EnemyController.levelComplete -= triggerEndOfWave;
    }

    IEnumerator wave() {
        while(true) {
            waveDone = false;
            for(int i = 0; i < monstersPerWave; i++) {
                var point = getRelevantSpawnPoint();
                ec.enemies.Add(Instantiate(enemies[Random.Range(0, enemies.Count)], point.position, Quaternion.identity, getRelevantSpawnPoint()));
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawntime));
            }
            while(!waveDone)
                yield return new WaitForSeconds(1f);
            monstersPerWave += enemyNumberIncrease;
            yield return new WaitForSeconds(timeBtwWaves);    //  TIME BTW WAVES
        }
    }

    Transform getRelevantSpawnPoint() {
        List<Transform> useables = new List<Transform>();
        for(int i = 0; i < 3; i++) {
            var u = spawnPoints.FindClosest(playerTrans.position);
            useables.Add(u);
            spawnPoints.RemoveAll(x => x.gameObject.GetInstanceID() == u.gameObject.GetInstanceID());
        }
        foreach(var i in useables)
            spawnPoints.Add(i);
        return useables[Random.Range(0, useables.Count)];
    }

    void triggerEndOfWave() {
        waveDone = true;
    }
}