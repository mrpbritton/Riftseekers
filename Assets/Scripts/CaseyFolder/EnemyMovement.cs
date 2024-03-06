using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private float rotationSpeed = 2f;
    [SerializeField]
    public NavMeshAgent agent;
    public bool bCover, bAttacking;
    private GameObject Player;
    private RaycastHit hitInfo;
    [SerializeField]
    private List<GameObject> cover = new List<GameObject>();
    private float close = 9999;
    public LayerMask enemy;
    public float coverTime = 3f;
    [SerializeField]
    public float hitCooldown = 1, enemySpeed, stopDistance;
    public GameObject target = null;

    bool canMove = true;
    Coroutine slider = null;


    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cover.Clear();
        cover.AddRange(GameObject.FindGameObjectsWithTag("Cover"));
        enemySpeed = agent.speed;
    }

    void Update()
    {
        if(!canMove) return;
        if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out hitInfo, 9999, enemy))
        {
            if (hitInfo.transform.CompareTag("Player") && !bCover)
            {
                pSeen();
            }
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

    public void lookForCover()
    {
        bCover = true;
        StartCoroutine(nameof(TimeInCover));


        close = 9999;
        foreach (GameObject current in cover)
        {

            float distance = Vector3.Distance(transform.position, current.transform.position);

            if (distance <= 20)
            {
                if (Physics.Raycast(current.transform.position, Player.transform.position - current.transform.position, out hitInfo, 999, enemy))
                {
                    if (!hitInfo.transform.CompareTag("Player"))
                    {
                        if (distance < close)
                        {
                            agent.stoppingDistance = 0;
                            close = distance;
                            target = current;
                        }
                    }
                }
            }

        }
    }


    IEnumerator TimeInCover()
    {
        yield return new WaitForSeconds(coverTime);
        bCover = false;
        target = null;
    }

    public void pSeen()
    {
        if (bAttacking) return;
        target = Player;
        agent.speed = enemySpeed;
        agent.stoppingDistance = stopDistance;
    }

    public void slide(Vector3 dir, float force, float time) {
        if(slider != null)
            return;
        slider = StartCoroutine(slideWaiter(dir, force, time));
    }
    IEnumerator slideWaiter(Vector3 dir, float force, float time) {
        canMove = false;
        float dTimeRemaining = time;

        while(dTimeRemaining > 0) {
            dTimeRemaining -= Time.deltaTime;
            agent.Move(force * Time.deltaTime * dir.normalized);
            yield return null;
        }

        canMove = true;
        slider = null;
    }
}
