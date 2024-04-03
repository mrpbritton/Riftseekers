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
    public bool bAttacking;
    private GameObject Player;
    private RaycastHit hitInfo;
    [SerializeField]
    private float close = 9999;
    public LayerMask enemy;
    [SerializeField]
    public float hitCooldown = 1, enemySpeed, stopDistance;
    public GameObject target = null;

    bool canMove = true;
    Coroutine slider = null;

    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemySpeed = agent.speed;
        target = Player;
    }

    void Update()
    {
        if(!canMove) return;

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

    public void slide(Vector3 dir, float force, float time) {
        if(slider != null || !gameObject.activeInHierarchy)
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
