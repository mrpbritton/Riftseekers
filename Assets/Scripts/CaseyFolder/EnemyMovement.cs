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
    [SerializeField]
    private bool bMelee, bCover;
    private GameObject target = null, Player, Gun;
    private RaycastHit hitInfo;
    [SerializeField]
    private List<GameObject> cover = new List<GameObject>();
    private float close = 9999;
    public LayerMask enemy;


    //    [SerializeField]
    //    private int coverTime = 1;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cover.AddRange(GameObject.FindGameObjectsWithTag("Cover"));
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out hitInfo, enemy))
        {
            if (hitInfo.transform.CompareTag("Player"))
            {
                pSeen();
            }
        }
//        lookForCover();

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
        close = 9999;
        foreach (GameObject current in cover)
        {
            float distance = Vector3.Distance(transform.position, current.transform.position);

            if (distance < close && current.activeSelf == true)
            {
                close = distance;
                target = current;
                Debug.Log(target);
            }
        }
    }

    public void leaveCover()
        {
            Debug.Log("leaving cover");
            target = Player;

    //        Debug.Log("Player found");
        }

        IEnumerator TimeInCover()
        {
            yield return new WaitForSeconds(1);
        }
    public void pSeen()
    {
        target = Player;
    }
}
