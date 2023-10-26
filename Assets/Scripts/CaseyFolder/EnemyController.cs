using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    public float difficulty = 1;
    private int loot;
    [SerializeField]
    private GameObject Item;
    [SerializeField]
    PlayerUICanvas enemySlider;
    public float total;


    public static Action levelComplete = delegate { };


    void Start()
    {
        enemySlider = FindObjectOfType<PlayerUICanvas>();
        foreach(var i in FindObjectsOfType<PlayerUICanvas>())
            if(i.transform.position.y > enemySlider.transform.position.y)
                enemySlider = i;
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        loot = UnityEngine.Random.Range(0, enemies.Count);
        enemies[loot].GetComponent<EnemyHealth>().hasItem = true;
        total = enemies.Count;
    }

    private void OnEnable()
    {
        EnemyHealth.onEnemyDeath += onEnemyDeath;
/*        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().maxhealth *= difficulty;
            enemy.GetComponent<Damage_GA>().damage *= difficulty;
        }
*/
    }

    private void OnDisable()
    {
        EnemyHealth.onEnemyDeath -= onEnemyDeath;
    }

    private void onEnemyDeath(GameObject deadEnemy)
    {
        if (deadEnemy.GetComponent<EnemyHealth>().hasItem)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(Item) as GameObject;
            go.transform.position = deadEnemy.transform.position;
        }
        enemies.Remove(deadEnemy);
        if(enemies.Count == 0)
        {
            //activate level complete sequence.
            Debug.Log("you win");
            levelComplete();
        }
        enemySlider.updateSlider(total, enemies.Count);
    }
}
