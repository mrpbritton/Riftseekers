using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowrdDamage : MonoBehaviour
{
    public float damage;
    public float range;
    Transform playerTrans;
    void Start()
    {
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.damageTaken(5, playerTrans.position);
        }
    }
    public void RangeUpgrade(int newRange)
    {

    }
}
