using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ExplosiveBarrelIntsance : MonoBehaviour {
    [SerializeField] ExplosionManager.explosionState state;
    [SerializeField] float radius;
    [SerializeField] float dmg;
    [SerializeField] float knockback;
    [SerializeField] UnityEvent explosionEvent;

    SphereCollider coll;


    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player" && (state == ExplosionManager.explosionState.HurtsAll || state == ExplosionManager.explosionState.HurtsPlayer)) {
            col.gameObject.GetComponent<Health>().takeDamage(dmg);
            var offset = transform.position - col.gameObject.transform.position;
            offset.y = 0f;
            col.gameObject.GetComponent<CharacterController>().Move(-(offset.normalized * knockback));
        }
        else if(col.gameObject.tag == "Enemy" && (state == ExplosionManager.explosionState.HurtsAll || state == ExplosionManager.explosionState.HurtsEnemies))
            col.gameObject.GetComponent<EnemyHealth>().damageTaken(dmg, transform.position);
    }

    private void Start() {
        coll = GetComponent<SphereCollider>();
        coll.radius = radius;
        coll.enabled = false;
        explosionEvent.AddListener(delegate { FindObjectOfType<ExplosionManager>().explode(transform.position, radius, ExplosionManager.explosionState.HurtsAll); });
        explosionEvent.AddListener(delegate { coll.enabled = true; Invoke("resetCol", .15f); });
    }

    public void triggerExplosion() {
        explosionEvent.Invoke();
    }
    void resetCol() {
        coll.enabled = false;
    }
}
