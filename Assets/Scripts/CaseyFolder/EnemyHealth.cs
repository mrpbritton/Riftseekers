using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float maxhealth;
    public float currentHealth;

    Coroutine invincTimer = null;
    float invincTime = .5f;

    [SerializeField] Collider usedCollider;
    [SerializeField] Slider healthSlider;

    private void Awake()
    {
        currentHealth = maxhealth;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = currentHealth;
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
    }
    private void deathAnimation()
    {
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
