using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {
    public float maxhealth;
    public float currentHealth;
    private float critHealth;
    private bool foundCover, bDead;
    public bool hasItem, firstDamage, hit, bStunned;
    [SerializeField]
    private float stunTime = 0.5f;
    private float timeStunned = 0;
    private Transform stunStart, stunEnd;

    /*    Coroutine invincTimer = null;
        float invincTime = .5f;*/

    [SerializeField] Collider usedCollider;
    [SerializeField] Slider healthSlider;

    public static Action<Transform> onEnemyDeath = delegate { };
    public static Action<Transform, Vector3, float> onEnemyHit = delegate { };

    private void Start()
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
        onEnemyHit(transform, attackPoint, damage);

        if(currentHealth <= 0)
        {
            if (bDead) return;
            bDead = true;
            deathAnimation();

            AkSoundEngine.PostEvent("Enemy_Death", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Enemy_Hit", gameObject);
        }


        if (currentHealth <= critHealth && !foundCover && UnityEngine.Random.Range(0, 1) < 1)
        {
            if (GetComponent<EnemyMelee>())
                return;
/*
            if(gameObject.TryGetComponent(out EnemyMovement movement))
            {
                movement.lookForCover();             
                foundCover = true;
            }
*/
        }

        if (!bStunned)
        {
            StartCoroutine(nameof(stunned));
            bStunned = true;
            stunStart = transform;
            stunEnd = transform;
            stunEnd.position += -transform.forward * 2;
            timeStunned = 0f;
        }

        if (timeStunned < stunTime)
        {
            transform.position = Vector3.Lerp(stunStart.position, stunEnd.position, timeStunned);
            timeStunned += Time.deltaTime;
        }

    }
    private void deathAnimation()
    {
        onEnemyDeath(transform);
        Destroy(gameObject);
    }

    IEnumerator stunned()
    {
        if(!GetComponent<EnemyMelee>()) GetComponentInChildren<EnemyFiring>().bStunned = true;
        GetComponent<EnemyMovement>().agent.speed = 0;
        yield return new WaitForSeconds(stunTime);
        if (!GetComponent<EnemyMelee>()) GetComponentInChildren<EnemyFiring>().bStunned = false;
        GetComponent<EnemyMovement>().agent.speed = GetComponent<EnemyMovement>().enemySpeed;
        bStunned = false;
    }


    /*    IEnumerator invincibilityWaiter(float timeOnInvinc) {
            usedCollider.enabled = false;
            yield return new WaitForSeconds(timeOnInvinc);
            usedCollider.enabled = true;
            invincTimer = null;
        }*/
}
