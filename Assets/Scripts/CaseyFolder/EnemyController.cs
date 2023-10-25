using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    public float difficulty = 1;
    private int loot;
    [SerializeField]
    private GameObject Item;

    public static Action levelComplete = delegate { };

    void Start()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        loot = UnityEngine.Random.Range(0, enemies.Count);
        enemies[loot].GetComponent<EnemyHealth>().hasItem = true;
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
    }
}
