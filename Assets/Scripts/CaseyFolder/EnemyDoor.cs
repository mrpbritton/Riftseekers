using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    [SerializeField]
    private Transform section1, section2;
    private float startpos1, startpos2;
    private float endpos1, endpos2;
    [SerializeField]
    private bool bToggle, bOpen, bMoving;
    [Tooltip("Time in seconds to open")]
    [SerializeField]
    private float doorSpeed = 3;
    private float timeElapsed;

    void Start()
    {
        startpos1 = section1.localPosition.x;
        startpos2 = section2.localPosition.x;
        endpos1 = startpos1 - 1.5f;
        endpos2 = startpos2 + 1.5f;
    }

    private void OnEnable()
    {
//        WaveSpawner.openDoor += openDoor;
//        WaveSpawner.closeDoor += closeDoor;
    }
    private void OnDisable()
    {
//        WaveSpawner.openDoor -= openDoor;
//        WaveSpawner.closeDoor -= closeDoor;
    }

    void Update()
    {
        if(bToggle)
        {
            bToggle = false;
            if (bMoving)
                return;
            if (bOpen)
                closeDoor();
            else
                openDoor();
        }

        if(bMoving)
        {
            if(bOpen)
            {
                if (timeElapsed < doorSpeed)
                {
                    section1.localPosition = new Vector3(Mathf.Lerp(endpos1, startpos1, timeElapsed / doorSpeed), 0, 0);
                    section2.localPosition = new Vector3(Mathf.Lerp(endpos2, startpos2, timeElapsed / doorSpeed), 0, 0);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    bMoving = false;
                    bOpen = false;
                    timeElapsed = 0;
                }
            }
            else
            {
                if (timeElapsed < doorSpeed)
                {
                    section1.localPosition = new Vector3(startpos1 + Mathf.Lerp(startpos1, endpos1, timeElapsed / doorSpeed), 0, 0);
                    section2.localPosition = new Vector3(startpos2 + Mathf.Lerp(startpos2, endpos2, timeElapsed / doorSpeed), 0, 0);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    bMoving = false;
                    bOpen = true;
                    timeElapsed = 0;
                }
            }
        }
    }

    public void openDoor()
    {
        bMoving = true;
        timeElapsed = 0;
    }

    public void closeDoor()
    {
        bMoving = true;
        timeElapsed = 0;
    }
}
