using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


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
    private float eRange = 10f;
    private GameObject Player;


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
        if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out hitInfo, eRange))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                bSeePlayer = true;
            }
            else
            {
                bSeePlayer = false;
            }
        }
    }

}
