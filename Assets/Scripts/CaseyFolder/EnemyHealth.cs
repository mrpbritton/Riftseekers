using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float maxhealth;
    public float currentHealth;
    private float critHealth;
    private bool foundCover;
    public bool hasItem, firstDamage;

/*    Coroutine invincTimer = null;
    float invincTime = .5f;*/

    [SerializeField] Collider usedCollider;
    [SerializeField] Slider healthSlider;

    public static Action<GameObject> onEnemyDeath = delegate { };
    public static Action<Transform, Vector3> onEnemyHit = delegate { };

    private void Awake()
    {
        currentHealth = maxhealth;
        critHealth = maxhealth * 0.5f;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = currentHealth;
    }

    public void damageTaken(float damage, Vector3 attackPoint)
    {
        if(!firstDamage)
        {
            GetComponentInChildren<Canvas>().enabled = true;
            firstDamage = true;
        }
        //  checks if invincible
/*        if(invincTimer != null)
            return;
        invincTimer = StartCoroutine(invincibilityWaiter(invincTime));*/
        currentHealth -= damage;

        healthSlider.value = currentHealth;
        onEnemyHit(transform, attackPoint);

        if(currentHealth <= 0)
        {
            deathAnimation();

            AkSoundEngine.PostEvent("Enemy_Death", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Enemy_Hit", gameObject);
        }


        if (currentHealth <= critHealth && !foundCover)
        {
            if(gameObject.TryGetComponent(out EnemyMovement movement))
            {
                movement.bCover = true;
                foundCover = true;
            }
        }

    }
    private void deathAnimation()
    {
        onEnemyDeath(gameObject);
        Destroy(gameObject);
    }

/*    IEnumerator invincibilityWaiter(float timeOnInvinc) {
        usedCollider.enabled = false;
        yield return new WaitForSeconds(timeOnInvinc);
        usedCollider.enabled = true;
        invincTimer = null;
    }*/
}
