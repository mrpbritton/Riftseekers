using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoomCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WaveSpawner.I.setCanManualStartWave(false);
    }

    private void OnTriggerExit(Collider other)
    {
        WaveSpawner.I.triggerImmediateWave();
        WaveSpawner.I.setCanManualStartWave(true);
    }
}
