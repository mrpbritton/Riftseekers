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
    public bool hasItem;

    Coroutine invincTimer = null;
    float invincTime = .5f;

    [SerializeField] Collider usedCollider;
    [SerializeField] Slider healthSlider;

    public static Action<GameObject> onEnemyDeath = delegate { };

    private void Awake()
    {
        currentHealth = maxhealth;
        critHealth = maxhealth * 0.5f;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = currentHealth;
        onEnemyDeath += delegate { FindObjectOfType<VisualFX>().enemyDeathFX(gameObject); };
    }

    private void OnDisable() {
        onEnemyDeath = new Action<GameObject>(delegate { });
    }

    public void damageTaken(float damage)
    {
        //  checks if invincible
        if(invincTimer != null)
            return;
        invincTimer = StartCoroutine(invincibilityWaiter(invincTime));
        currentHealth -= damage;

        healthSlider.value = currentHealth;

        if(currentHealth <= 0)
        {
            deathAnimation();
        }

        if(currentHealth <= critHealth && !foundCover)
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
        Debug.Log("death");
        Destroy(gameObject);
    }

    IEnumerator invincibilityWaiter(float timeOnInvinc) {
        usedCollider.enabled = false;
        yield return new WaitForSeconds(timeOnInvinc);
        usedCollider.enabled = true;
        invincTimer = null;
    }
}
