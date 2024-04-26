using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    bool waveTriggered = false;

    [Tooltip("Enemies that will be spawned")]
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    int enemiesInPool = 500;
    List<List<GameObject>> enemyPools = new List<List<GameObject>>();

    KdTree<Transform> spawnPoints = new KdTree<Transform>();

    EnemyController ec;
    Transform playerTrans;

    [SerializeField] Transform pParent;
    [SerializeField] CircularSlider waveTriggerSlider;
    [SerializeField] Transform nextWaveText;
    [SerializeField] TextMeshProUGUI waveTriggerText;

    PInput pInput;

    [HideInInspector] public int waveIndex = 0;

    Coroutine triggerWaiter = null;
    Coroutine waver = null;

    public static System.Action WaveComplete = delegate { };
    public static System.Action WaveStart = delegate { };
    private void Start() {
        pInput = new PInput();
        pInput.Enable();
        pInput.Player.TriggerWave.performed += ctx => triggerWave();
        pInput.Player.TriggerWave.canceled += ctx => {
            if(waveTriggered) return;
            waveTriggerSlider.doValueKill();
            waveTriggerSlider.doValue(0f, .25f, true);
        };

        waveTriggerSlider.setValue(0f);
        nextWaveText.gameObject.SetActive(false);

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

        //  adds enemies to pools
        int index = 0;
        foreach(var i in enemies) {
            enemyPools.Add(new List<GameObject>());
            for(int j = 0; j < enemiesInPool; j++) {
                var temp = Instantiate(i.gameObject, pParent);
                temp.SetActive(false);
                enemyPools[index].Add(temp);
            }
            index++;
        }

        waveIndex = Inventory.getWaveIndex();
        rampUp(false);

        WaveCounter.I.UpdateCounter();
    }

    private void OnDisable() {
        pInput.Disable();
        EnemyController.levelComplete -= triggerEndOfWave;
    }

    IEnumerator wave() {
        WaveStart();
        yield return new WaitForSeconds(1f);
        //  checks if needs to do tutorial
        if(SaveData.getInt("Tutorial", 0) == 0) {
            for(int i = 0; i < enemies.Count; i++) {
                var point = getRelevantSpawnPoint();
                var temp = enemyPools[i][0];
                enemyPools[i].RemoveAt(0);
                temp.SetActive(true);
                temp.transform.position = point.position;
                ec.enemies.Add(temp);
                while(temp.activeInHierarchy)
                    yield return new WaitForSeconds(1f);
            }
            WaveOverCanvas.I.runAnim();
        }
        SaveData.setInt("Tutorial", 1);

        while(true) {
            WaveStart();
            waveDone = false;
            for(int i = 0; i < monstersPerWave; i++) {
                var point = getRelevantSpawnPoint();
                int rand = Random.Range(0, enemyPools.Count);
                var temp = enemyPools[rand][0];
                enemyPools[rand].RemoveAt(0);
                temp.SetActive(true);
                temp.transform.position = point.position;
                ec.enemies.Add(temp);
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawntime));
            }
            while(!waveDone)
                yield return new WaitForSeconds(1f);

            rampUp(true);
            Inventory.setWaveIndex(waveIndex);
            Inventory.saveInventory();
            waveTriggered = false;

            //  waits for player to trigger next wave
            yield return new WaitForSeconds(2f);
            Debug.Log("here");
            if(triggerWaiter != null)
                StopCoroutine(triggerWaiter);
            triggerWaiter = StartCoroutine(triggerWaveWaiter());
            nextWaveText.gameObject.SetActive(true);
            waveTriggerText.text = "hold<color=yellow>" + (InputManager.isUsingKeyboard() ? " z " : " a ") + "<color=white>for next wave";
            while(!waveTriggered)
                yield return new WaitForSeconds(1f);
        }
        waver = null;
    }

    public void triggerWave() {
        if(waveTriggered) return;
        waveTriggerText.text = "hold<color=yellow>" + (InputManager.isUsingKeyboard() ? " z " : " a ") + "<color=white>for next wave";
        waveTriggerSlider.doValueKill();
        waveTriggerSlider.setValue(0f);
        waveTriggerSlider.setColor(Color.yellow);
        waveTriggerSlider.doColor(Color.white, 1f);
        waveTriggerSlider.doValue(1f, 1f, false, delegate {
            waveTriggered = true;
            waveTriggerSlider.setValue(0f);
            nextWaveText.gameObject.SetActive(false);
            waveIndex++;
            WaveCounter.I.UpdateCounter();
            triggerWaiter = null;
        });
    }
    public void triggerImmediateWave() {
        if(waver == null)
            waver = StartCoroutine(wave());
        waveTriggered = true;
        waveTriggerSlider.setValue(0f);
        nextWaveText.gameObject.SetActive(false);
        triggerWaiter = null;
    }
    public void setCanManualStartWave(bool b) {
        if(b)
            pInput.Player.TriggerWave.performed += ctx => triggerWave();
        else
            pInput.Player.TriggerWave.performed -= ctx => triggerWave();
    }

    IEnumerator triggerWaveWaiter() {
        yield return new WaitForSeconds(2f);
        nextWaveText.transform.DOPunchScale(Vector3.one * .5f, .35f);
        triggerWaiter = null;
    }

    void rampUp(bool runCompete) {
        if(waveIndex < 5) {
            monstersPerWave = Mathf.Clamp(monstersPerWave + (int)(startingMonstersPerWave * Mathf.Pow(roundBase, waveIndex)), 0, enemiesInPool);
            minSpawnTime *= spawnTimeInc;
            maxSpawntime *= spawnTimeInc;
        }
        else {
            foreach(var i in enemies) {
                i.GetComponent<EnemyHealth>().maxhealth = i.GetComponent<EnemyHealth>().baseHealth + (5 * waveIndex);
                i.transform.localScale = Vector3.one * (0.25f * waveIndex);
                i.GetComponentInChildren<Damage_GA>().damage = i.GetComponentInChildren<Damage_GA>().originalDamage * (.5f * waveIndex);
            }
        }
        if(runCompete)
            WaveComplete();
    }

    Transform getRelevantSpawnPoint() {
        List<Transform> useables = new List<Transform>();
        for(int i = 0; i < Mathf.Min(3, spawnPoints.Count); i++) {
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

    public void repoolEnemy(GameObject obj) {
        for(int i = 0; i < enemies.Count; i++) {
            if(obj.name == enemies[i].name) {
                enemyPools[i].Add(obj);
                return;
            }
        }
    }
}