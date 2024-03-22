using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    private GameObject Player;
    [SerializeField]
    private GameObject meleeHit;
    [SerializeField]
    private NavMeshAgent agent;
    public bool bAttacking, bDashAttack, bAttackCooldown;
    [SerializeField]
    public float hitCooldown = 1, coverTime = 3f;


    private void Start()
    {
        Inventory.clear();
        Player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<EnemyMovement>().coverTime = coverTime;
        GetComponent<EnemyMovement>().stopDistance = agent.stoppingDistance;
    }

    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= 5 && !bAttacking && !bAttackCooldown)
        {
            AkSoundEngine.PostEvent("Enemy_Melee", gameObject);
            bAttacking = true;
            if (UnityEngine.Random.Range(0, 3) < 1)
                StartCoroutine(nameof(dashAttack));
            else
                StartCoroutine(nameof(basicAttack));
        }
        if(bDashAttack)
        {
            transform.position += transform.forward * 20 * Time.deltaTime;
        }
    }

    IEnumerator basicAttack()
    {
        bAttackCooldown = true;
        GetComponent<EnemyMovement>().target = null;
        GetComponent<EnemyMovement>().bAttacking = true;
        agent.speed = 0;
        yield return new WaitForSeconds(hitCooldown);
        meleeHit.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        meleeHit.GetComponent<Collider>().enabled = false;
        agent.speed = GetComponent<EnemyMovement>().enemySpeed;
        bAttacking = false;
        GetComponent<EnemyMovement>().bAttacking = false;
        yield return new WaitForSeconds(hitCooldown);
        GetComponent<EnemyMovement>().target = Player;
        bAttackCooldown = false;
    }

    IEnumerator dashAttack()
    {
        bAttackCooldown = true;
        GetComponent<EnemyMovement>().target = null;
        GetComponent<EnemyMovement>().bAttacking = true;
        agent.speed = 0;
        yield return new WaitForSeconds(hitCooldown);
        bDashAttack = true;
        meleeHit.GetComponent<Collider>().enabled = true;

        yield return new WaitForSeconds(0.25f);
        bDashAttack = false;
        meleeHit.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1);
        agent.speed = GetComponent<EnemyMovement>().enemySpeed;
        bAttacking = false;
        GetComponent<EnemyMovement>().bAttacking = false;
        yield return new WaitForSeconds(hitCooldown);
        GetComponent<EnemyMovement>().target = Player;
        bAttackCooldown = false;
    }

}
