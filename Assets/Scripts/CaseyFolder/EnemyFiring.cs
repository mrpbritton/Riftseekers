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
    public bool bSeePlayer, bStunned, firing, bShotgun;
    private RaycastHit hitInfo;
    [SerializeField]
    private GameObject Player;
    public LayerMask enemy;
    [SerializeField]
    private NavMeshAgent agent;
    private float spread = 0.3f;
    private Vector3 cachedDir = new(1, 1, 1);



    private void OnEnable()
    {
        StartCoroutine(nameof(Reloading));
        Player = GameObject.FindGameObjectWithTag("Player");
        GetComponentInParent<EnemyMovement>().stopDistance = agent.stoppingDistance;
    }
    private void FireShot()
    {
        lookForPlayer();
        if (bSeePlayer && !bStunned)
        {
            AkSoundEngine.PostEvent("Enemy_Fire", gameObject);
            rotation = gameObject.transform.rotation;
            Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
            project.Direction = transform.forward;
        }
        StartCoroutine(nameof(Reloading));

    }

    private void FireShotgun()
    {
        lookForPlayer();
        if (bSeePlayer && !bStunned)
        {
            AkSoundEngine.PostEvent("Enemy_Fire", gameObject);
            rotation = gameObject.transform.rotation;
            for (int i = 0; i < 4; i++)
            {
                float zRand = UnityEngine.Random.Range(-spread, spread);
                float xRand = UnityEngine.Random.Range(-spread, spread);
                Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
                project.Direction = new Vector3(transform.forward.x + xRand, transform.forward.y, transform.forward.z + zRand);
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

        if (!bShotgun)
            FireShot();
        else
            FireShotgun();
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(reloadTime);
        if(!bShotgun)
            FireShot();
        else
            FireShotgun();
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
