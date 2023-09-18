using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowrdDamage : MonoBehaviour
{
    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.damageTaken(5);
        }
    }
}
