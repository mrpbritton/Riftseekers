using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour {
    [SerializeField] bool destroyOnTrigger = false;
    [SerializeField] string[] tagsToLookFor;
    [SerializeField] UnityEvent events;

    private void OnTriggerEnter(Collider col) {
        if(tagsToLookFor.Contains(col.gameObject.tag)) {
            events.Invoke();
            if(destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
