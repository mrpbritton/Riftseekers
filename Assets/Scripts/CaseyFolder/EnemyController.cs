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
    void Start()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        loot = Random.Range(0, enemies.Count);
        enemies[loot].GetComponent<EnemyHealth>().hasItem = true;
    }

    private void OnEnable()
    {
        EnemyHealth.onEnemyDeath += onEnemyDeath;
        foreach(GameObject enemy in enemies)
        {

        }
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
    }
}
