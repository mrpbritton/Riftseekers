using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour {
    [SerializeField] string[] checkedTags;
    [SerializeField] UnityEvent ev;

    private void OnCollisionEnter(Collision col) {
        Debug.Log(col.gameObject.tag);
        if(checkedTags.Contains(col.gameObject.tag))
            ev.Invoke();
    }
}
