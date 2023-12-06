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
    private bool bSeePlayer = false;
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
        lookForPlayer();
        if (bSeePlayer)
        {
            rotation = gameObject.transform.rotation;
            Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
            project.Direction = transform.forward;
        }
        StartCoroutine(nameof(Reloading));
    }
    IEnumerator Reloading()
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
