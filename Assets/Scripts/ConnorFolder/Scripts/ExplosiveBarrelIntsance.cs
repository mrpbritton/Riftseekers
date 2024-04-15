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
    [SerializeField] GameObject animObj, barrelObj;
    [SerializeField] float respawnTime = 5f;

    Coroutine waiter = null;

    public void triggerExplosion() {
        if(waiter != null) return;
        ExplosionManager.I.explode(transform.position, radius, dmg, maxKnockback, ExplosionManager.explosionState.HurtsAll);
        StartCoroutine(explodeWaiter());
    }

    IEnumerator explodeWaiter() {
        barrelObj.SetActive(false);
        animObj.SetActive(true);
        yield return new WaitForSeconds(respawnTime);
        barrelObj.SetActive(true);
        animObj.SetActive(false);

    }
}
