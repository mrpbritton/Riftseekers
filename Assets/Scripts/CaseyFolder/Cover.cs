using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> coverList = new List<GameObject>();
    private RaycastHit hitInfo;
    private GameObject Player;
    public LayerMask enemy;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        coverSearch();
    }

    private void coverSearch()
    {
        foreach(GameObject current in coverList)
        {
            if (Physics.Raycast(current.transform.position, Player.transform.position - current.transform.position, out hitInfo, enemy))
            {
                if (hitInfo.transform.CompareTag("Player"))
                {
                    current.SetActive(false);
                }
                else
                {
                    current.SetActive(true);
                }
            }
        }
        StartCoroutine(nameof(waitTime));
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(2);
        coverSearch();
    }
}
