using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : Singleton<WaveSpawner> {
    [Header("Spawning Information")]
    [Tooltip("How many enemies will spawn")]
    [SerializeField] int startingMonstersPerWave = 20;
    int monstersPerWave;
    [SerializeField] float roundBase = 1.78f;
    [SerializeField] float timeBtwWaves = 10f;
    [Tooltip("How long it takes for an enemy to spawn")]
    [SerializeField] float minSpawnTime = .15f, maxSpawntime = .35f;
    [SerializeField] float enemyNumberInc = 1.25f;
    [SerializeField] float spawnTimeInc = .9f;
    bool waveDone = false;

    [Tooltip("Enemies that will be spawned")]
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    KdTree<Transform> spawnPoints = new KdTree<Transform>();

    EnemyController ec;
    Transform playerTrans;

    public static System.Action WaveComplete = delegate { };
    private void Start() {
        EnemyController.levelComplete += triggerEndOfWave;
        ec = FindObjectOfType<EnemyController>();
        for(int i = 0; i < transform.childCount; i++)
            spawnPoints.Add(transform.GetChild(i));
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
        monstersPerWave = startingMonstersPerWave;


        foreach(var i in enemies) {
            i.GetComponent<EnemyHealth>().maxhealth = i.GetComponent<EnemyHealth>().baseHealth;
            i.GetComponent<EnemyHealth>().currentHealth = i.GetComponent<EnemyHealth>().maxhealth;
            if(i.GetComponentInChildren<Damage_GA>() != null)
                i.GetComponentInChildren<Damage_GA>().damage = i.GetComponentInChildren<Damage_GA>().originalDamage;
            i.transform.localScale = Vector3.one;
        }

        StartCoroutine(wave());
    }

    private void OnDisable() {
        EnemyController.levelComplete -= triggerEndOfWave;
    }

    IEnumerator wave() {
        yield return new WaitForSeconds(5f);
        while(true) {
            waveDone = false;
            for(int i = 0; i < monstersPerWave; i++) {
                var point = getRelevantSpawnPoint();
                ec.enemies.Add(Instantiate(enemies[Random.Range(0, enemies.Count)], point.position, Quaternion.identity, getRelevantSpawnPoint()));
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawntime));
            }
            while(!waveDone)
                yield return new WaitForSeconds(1f);

            rampUp();
            yield return new WaitForSeconds(timeBtwWaves);    //  TIME BTW WAVES
        }
    }

    void rampUp() {
        if(WaveCounter.I.WaveNum < 5) {
            monstersPerWave += (int)(startingMonstersPerWave * Mathf.Pow(roundBase, WaveCounter.I.WaveNum));
            minSpawnTime *= spawnTimeInc;
            maxSpawntime *= spawnTimeInc;
        }
        else {
            foreach(var i in enemies) {
                i.GetComponent<EnemyHealth>().maxhealth = i.GetComponent<EnemyHealth>().baseHealth + (5 * WaveCounter.I.WaveNum);
                i.transform.localScale = Vector3.one * (0.25f * WaveCounter.I.WaveNum);
                i.GetComponentInChildren<Damage_GA>().damage = i.GetComponentInChildren<Damage_GA>().originalDamage * (.5f *  WaveCounter.I.WaveNum);
            }
        }
        WaveComplete();
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