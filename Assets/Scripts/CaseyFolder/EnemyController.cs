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
    [SerializeField]
    PlayerUICanvas enemySlider;
    public float total;
    [SerializeField]
    private int baseMoney, moneyVariance;
    [SerializeField]
    [Range(0, 100)]
    private float dropChance, healthChance, augmentChance, loreChance, abilityChance;


    public static Action levelComplete = delegate { };


    void Start()
    {
        enemySlider = FindObjectOfType<PlayerUICanvas>();
        foreach(var i in FindObjectsOfType<PlayerUICanvas>())
            if(i.transform.position.y > enemySlider.transform.position.y)
                enemySlider = i;
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        total = enemies.Count;
        enemySlider.updateSlider(total, enemies.Count);
    }

    private void OnEnable()
    {
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
                    //drop chance per rarity of augments
                    switch(UnityEngine.Random.Range(0, 100))
                    {
                        case int m when (m >= 0 && m < 60):
                            Debug.Log("common augment");
                            break;
                        case int m when (m >= 60 && m < 90):
                            Debug.Log("uncommon augment");
                            break;
                        case int m when (m >= 90 && m <= 100):
                            Debug.Log("rare augment");
                            break;
                    }
                    int a = UnityEngine.Random.Range(0, AugmentLibrary.I.getAugments().Count);
                    GameObject aug = Instantiate(augmentItem);
                    aug.GetComponent<AddAugment_GA>().refType = AugmentLibrary.I.getAugment(a).type;
                    aug.transform.position = deadEnemy.transform.position;
                    
                    break;

                case int n when (n >= healthChance + augmentChance && n < healthChance + augmentChance + loreChance):
                    Debug.Log("lore");
                    /*
                    int l = UnityEngine.Random.Range(0, AugmentLibrary.I.getLore().Count);
                    GameObject lor = Instantiate(AugmentLibrary.I.getLore[i]);
                    lor.transform.position = deadEnemy.transform.position;
                    */
                    break;

                case int n when (n >= healthChance + augmentChance + loreChance && n < healthChance + augmentChance + loreChance + abilityChance):

//                    int i = UnityEngine.Random.Range(0, AttackLibrary.I.getItems().Count);
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

        enemySlider.enemyBar(total, enemies.Count);
        Inventory.changeMoney(UnityEngine.Random.Range(baseMoney-moneyVariance, baseMoney+moneyVariance));
    }
}
