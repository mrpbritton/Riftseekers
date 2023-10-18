using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 2f;
    [SerializeField]
    private NavMeshAgent agent;
    public bool bMelee, bCover, bClose;
    [SerializeField]
    private GameObject target = null, Gun, Player, meleeHit;
    private RaycastHit hitInfo;
    [SerializeField]
    private List<GameObject> cover = new List<GameObject>();
    private float close = 9999;
    public LayerMask enemy;
    [SerializeField]
    private int coverTime = 10, hitCooldown = 1;




    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cover.AddRange(GameObject.FindGameObjectsWithTag("Cover"));
        meleeAttack();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out hitInfo, 9999, enemy))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                pSeen();
            }
        }

        if (bCover && !bMelee)
        {
            lookForCover();
            StartCoroutine(nameof(TimeInCover));
        }

        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 90f - angle, 0f));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            agent.SetDestination(target.transform.position);
        }
    }

        //change target position to cover
    public void lookForCover()
    {
        if (!bMelee)
        {
            agent.stoppingDistance = 0;
        }

        close = 9999;
        foreach (GameObject current in cover)
        {
            float distance = Vector3.Distance(transform.position, current.transform.position);

            if (distance < close && current.activeSelf == true)
            {
                close = distance;
                target = current;
            }
        }
    }


    IEnumerator TimeInCover()
    {
        yield return new WaitForSeconds(coverTime);
        bCover = false;
    }

    public void pSeen()
    {
        if(!bCover)
        {
            target = Player;
            if (!bMelee)
            {
                agent.stoppingDistance = 10;
            }

        }
    }

    private void meleeAttack()
    {
        if (!bMelee) return;
        if (Vector3.Distance(gameObject.transform.position, Player.transform.position) <= 3)
        {
            bClose = true;
        }
        else { bClose= false; }

        StartCoroutine(nameof(attackCooldown));
    }

    IEnumerator attackCooldown()
    {
        if (bClose)
        {
            meleeHit.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            meleeHit.SetActive(false);
        }
        yield return new WaitForSeconds(hitCooldown);
        meleeAttack();
    }

}
