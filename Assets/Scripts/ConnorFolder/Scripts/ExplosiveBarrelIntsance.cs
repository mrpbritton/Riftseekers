using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosiveBarrelIntsance : MonoBehaviour {
    [SerializeField] float radius;
    [SerializeField] UnityEvent explosionEvent;

    private void Start() {
        explosionEvent.AddListener(delegate { FindObjectOfType<ExplosionManager>().explode(transform.position, radius, ExplosionManager.explosionState.HurtsAll); });
    }

    public void triggerExplosion() {
        Debug.Log("Explosided");
        explosionEvent.Invoke();
    }
}
