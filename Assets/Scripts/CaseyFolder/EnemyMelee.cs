using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField]
    private GameObject Player, meleeHit;
    [SerializeField]
    private NavMeshAgent agent;
    public bool bAttacking;
    [SerializeField]
    public float hitCooldown = 1, coverTime = 3f;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        GetComponentInParent<EnemyMovement>().coverTime = coverTime;
    }

    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= 5 && !bAttacking)
        {
            AkSoundEngine.PostEvent("Enemy_Melee", gameObject);
            bAttacking = true;
            StartCoroutine(nameof(attackCooldown));
        }
    }

    IEnumerator attackCooldown()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(hitCooldown);
        meleeHit.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        meleeHit.GetComponent<Collider>().enabled = false;
        agent.speed = GetComponent<EnemyMovement>().enemySpeed;
        bAttacking = false;
    }
}
