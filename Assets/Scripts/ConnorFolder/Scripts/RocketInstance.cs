using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInstance : MonoBehaviour {
    Coroutine expCo;
    ExplosionManager em;
    float expSize;


    private void OnTriggerEnter(Collider col) {
        if(!col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Bullet")) {
            if(!col.gameObject.CompareTag("Enemy")) {
                AkSoundEngine.PostEvent("Object_Hit", gameObject);
            }
            Debug.Log(col.gameObject.tag);
            StopCoroutine(expCo);
            em.explode(transform.position, expSize, ExplosionManager.explosionState.HurtsEnemies);
            Destroy(gameObject, 0.0001f);
        }
    }

    public void setup(Coroutine ex, ExplosionManager e, float expS) {
        expCo = ex;
        em = e;
        expSize = expS;
    }
}
