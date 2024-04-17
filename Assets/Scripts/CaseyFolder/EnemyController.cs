using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    public float difficulty = 1;
    [SerializeField]
    private GameObject healthItem, augmentItem, loreItem, abilityItem;
    public float total;
    [SerializeField]
    private int baseMoney, moneyVariance;
    [SerializeField]
    [Range(0, 100)]
    private float dropChance, healthChance, augmentChance, loreChance, abilityChance;


    public static Action levelComplete = delegate { };

    private void OnEnable()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        total = enemies.Count;

        EnemyHealth.onEnemyDeath += onEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.onEnemyDeath -= onEnemyDeath;
    }

    private void onEnemyDeath(Transform deadEnemy)
    {

        if(UnityEngine.Random.Range(0, 100) < dropChance)
        {
            switch (UnityEngine.Random.Range(0, 100))
            {
                case int n when (n >= 0 && n < healthChance):
                    GameObject go = Instantiate(healthItem);
                    go.transform.position = deadEnemy.transform.position;
                    break;

                case int n when (n >= healthChance && n < healthChance + augmentChance):
                    GameObject aug = Instantiate(augmentItem);
                    aug.GetComponent<AddAugment_GA>().refType = AugmentLibrary.I.getRandAugment().type;
                    aug.transform.position = deadEnemy.transform.position;
                    
                    break;

                case int n when (n >= healthChance + augmentChance && n < healthChance + augmentChance + loreChance):
                    GameObject lor = Instantiate(loreItem);
                    lor.transform.position = deadEnemy.transform.position;
                    break;

                case int n when (n >= healthChance + augmentChance + loreChance && n < healthChance + augmentChance + loreChance + abilityChance):

                    GameObject abil = Instantiate(abilityItem);
//                    abil.GetComponent<AddItem_GA>().refType = AttackLibrary.I.getItem(i).type;
                    abil.transform.position = deadEnemy.transform.position;
                    break;
            }
        }
        enemies.Remove(deadEnemy.gameObject);
        if(enemies.Count == 0)
        {
            //activate level complete sequence.
            Debug.Log("you win");
            levelComplete();
        }

        Inventory.changeMoney(UnityEngine.Random.Range(baseMoney-moneyVariance, baseMoney+moneyVariance));
    }
}
