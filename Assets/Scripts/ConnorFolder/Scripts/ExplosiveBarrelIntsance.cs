using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ExplosiveBarrelIntsance : MonoBehaviour {
    [SerializeField] ExplosionManager.explosionState state;
    [SerializeField] float radius;
    [SerializeField] float dmg;
    [SerializeField] float maxKnockback;
    [SerializeField] UnityEvent explosionEvent;

    private void Start() {
        explosionEvent.AddListener(delegate { FindObjectOfType<ExplosionManager>().explode(transform.position, radius, dmg, maxKnockback, ExplosionManager.explosionState.HurtsAll); });
    }

    public void triggerExplosion() {
        explosionEvent.Invoke();
    }
}
