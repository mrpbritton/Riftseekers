using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    public float difficulty = 1;
    void Start()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
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

    }
    void Update()
    {
        
    }

    private void onEnemyDeath(GameObject deadEnemy)
    {
        enemies.Remove(deadEnemy);
    }
}
