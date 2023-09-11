using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 2f;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private bool bMelee;
    private GameObject target, Player, Cover, firePosition;
    [SerializeField]
    private int coverTime = 1;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

//        if (bMelee)
            target = Player;
    }

    void Update()
    {
//        Cover = GameObject.FindGameObjectWithTag("Cover");

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
        Debug.Log("looking for cover");
        target = Cover;
        StartCoroutine(nameof(TimeInCover));
    }

    public void leaveCover()
    {
        Debug.Log("leaving cover");
        target = Player;

//        Debug.Log("Player found");
    }

    IEnumerator TimeInCover()
    {
        yield return new WaitForSeconds(coverTime);
    }
    
}
