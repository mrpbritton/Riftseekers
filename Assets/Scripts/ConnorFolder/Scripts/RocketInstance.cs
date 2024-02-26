using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInstance : MonoBehaviour {
    Coroutine expCo;
    ExplosionManager em;
    float expSize;
    float dmg;
    float knockback;


    private void OnTriggerEnter(Collider col) {
        if(!col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Bullet")) {
            if(!col.gameObject.CompareTag("Enemy")) {
                AkSoundEngine.PostEvent("Object_Hit", gameObject);
            }
            if(expCo != null) {
                StopCoroutine(expCo);
                expCo = null;
            }
            em.explode(transform.position, expSize, dmg, knockback, ExplosionManager.explosionState.HurtsAll);
            Destroy(gameObject, 0.0001f);
        }
    }

    public void setup(Coroutine ex, ExplosionManager e, float expS, float expD, float expKn) {
        expCo = ex;
        em = e;
        expSize = expS;
        dmg = expD;
        knockback = expKn;
    }
}
