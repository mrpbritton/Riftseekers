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


    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player" && (state == ExplosionManager.explosionState.HurtsAll || state == ExplosionManager.explosionState.HurtsPlayer)) {
            col.gameObject.GetComponent<Health>().takeDamage(dmg);  //  deal damage

            //  push back
            var offset = transform.position - col.gameObject.transform.position;
            var distMod = Mathf.Clamp(1f - (Vector3.Distance(transform.position, col.gameObject.transform.position) / radius), 0f, 1f);
            offset.y = 0f;
            col.gameObject.GetComponent<CharacterController>().Move((-(offset.normalized * maxKnockback)) * distMod);
        }
        else if(col.gameObject.tag == "Enemy" && (state == ExplosionManager.explosionState.HurtsAll || state == ExplosionManager.explosionState.HurtsEnemies)) 
            col.gameObject.GetComponent<EnemyHealth>().damageTaken(dmg, transform.position);
    }

    private void Start() {
        explosionEvent.AddListener(delegate { FindObjectOfType<ExplosionManager>().explode(transform.position, radius, ExplosionManager.explosionState.HurtsAll); });
    }

    public void triggerExplosion() {
        explosionEvent.Invoke();
    }

    public void test() {
        Debug.Log("here");
    }
}
