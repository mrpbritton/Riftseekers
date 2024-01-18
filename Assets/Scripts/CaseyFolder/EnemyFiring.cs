using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



public class EnemyFiring : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float reloadTime = 2f;
    private Quaternion rotation = Quaternion.identity;
    public bool bSeePlayer, bStunned, firing;
    private RaycastHit hitInfo;
    [SerializeField]
    private GameObject Player;
    public LayerMask enemy;
    [SerializeField]
    private NavMeshAgent agent;




    private void Start()
    {
        StartCoroutine(nameof(Reloading));
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FireShot()
    {
        if(!GetComponentInParent<EnemyMovement>().bCover)
        {
            lookForPlayer();
            if (bSeePlayer && !bStunned)
            {
                AkSoundEngine.PostEvent("Enemy_Fire", gameObject);
                rotation = gameObject.transform.rotation;
                Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
                project.Direction = transform.forward;
            }
        }
        StartCoroutine(nameof(Reloading));

    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime * (1f/4f));
        firing = false;
        yield return new WaitForSeconds(reloadTime * (2f/4f));
        firing = true;
        yield return new WaitForSeconds(reloadTime * (1f/4f));

        FireShot();
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(reloadTime);
        FireShot();
    }

    private void lookForPlayer()
    {
        if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out hitInfo, 999, enemy))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                bSeePlayer = true;
                agent.stoppingDistance = 10;
            }
            else
            {
                bSeePlayer = false;
                agent.stoppingDistance = 0;
            }
        }
    }

}
