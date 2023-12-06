using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour {
    [SerializeField] bool destroyOnTrigger = false;
    [SerializeField] string[] tagsToLookFor;
    [SerializeField] UnityEvent events;

    [SerializeField] int minNumEnemiesKilledToTrigger = 0;
    int startEnemyCount;

    private void Start() {
        startEnemyCount = FindObjectsOfType<EnemyHealth>().Length;
    }

    private void OnTriggerEnter(Collider col) {
        if(tagsToLookFor.Contains(col.gameObject.tag) && startEnemyCount - FindObjectsOfType<EnemyHealth>().Length >= minNumEnemiesKilledToTrigger) {
            events.Invoke();
            if(destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
