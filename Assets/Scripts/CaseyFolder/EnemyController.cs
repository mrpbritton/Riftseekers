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
    [SerializeField]
    private GameObject healthItem, augmentItem;
    [SerializeField]
    PlayerUICanvas enemySlider;
    public float total;
    [SerializeField]
    private int baseMoney, moneyVariance;
    [SerializeField]
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

    private void onEnemyDeath(GameObject deadEnemy)
    {
    /*
            if(UnityEngine.Random.Range(0, 100) < dropChance)
            {
                Item = itemLibrary[UnityEngine.Random.Range(0, itemLibrary.count)];
                GameObject go = Instantiate(Item);
                go.transform.position = deadEnemy.transform.position;
            }
    */

        if(UnityEngine.Random.Range(0, 100) < dropChance)
        {
            switch (UnityEngine.Random.Range(0, 100))
            {
                case int n when (n >= 0 && n < healthChance):
                    GameObject go = Instantiate(healthItem);
                    go.transform.position = deadEnemy.transform.position;
                    break;

                case int n when (n >= healthChance && n < healthChance + augmentChance):
                    Debug.Log("augment");

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
                    Debug.Log("ability");

//                    int i = UnityEngine.Random.Range(0, AugmentLibrary.I.getItems().Count);
//                    GameObject abil = Instantiate(AugmentLibrary.I.getItem[i]);
//                    abil.transform.position = deadEnemy.transform.position;
                    break;
            }
        }
        enemies.Remove(deadEnemy);
/*        if(enemies.Count == 0)
        {
            //activate level complete sequence.
            Debug.Log("you win");
            levelComplete();
        }
*/
        enemySlider.updateSlider(total, enemies.Count);
        Inventory.changeMoney(UnityEngine.Random.Range(baseMoney-moneyVariance, baseMoney+moneyVariance));
    }
}
