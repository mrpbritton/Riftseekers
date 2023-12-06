using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour {
    [SerializeField] string[] checkedTags;
    [SerializeField] UnityEvent ev = new UnityEvent();

    private void OnTriggerEnter(Collider col) {
        if(checkedTags.Contains(col.gameObject.tag)) {
            ev.Invoke();
        }
    }

    private void OnCollisionEnter(Collision col) {
        if(checkedTags.Contains(col.gameObject.tag)) {
            ev.Invoke();
        }
    }
}
