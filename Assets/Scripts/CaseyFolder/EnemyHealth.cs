using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float maxhealth;
    public float currentHealth;
    private void Start()
    {
        currentHealth = maxhealth;
    }

    public void damageTaken(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            deathAnimation();
        }
    }
    private void deathAnimation()
    {
        Destroy(gameObject);
    }
}
