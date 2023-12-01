using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCollider : MonoBehaviour {
    [SerializeField] SphereCollider expCol;
    ExplosionManager.explosionState state;
    float dmg;
    float maxKnockback;


    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            if(state == ExplosionManager.explosionState.HurtsPlayer || state == ExplosionManager.explosionState.HurtsAll) {
                col.gameObject.GetComponent<Health>().takeDamage(dmg);  //  do damage
                //  push back
                var offset = transform.position - col.gameObject.transform.position;
                var distMod = Mathf.Clamp(1f - (Vector3.Distance(transform.position, col.gameObject.transform.position) / expCol.radius), 0f, 1f);
                offset.y = 0f;
                col.gameObject.GetComponent<CharacterController>().Move((-(offset.normalized * maxKnockback)) * distMod);
            }
        }
        else if(col.gameObject.tag == "Enemy") {
            if(state == ExplosionManager.explosionState.HurtsEnemies || state == ExplosionManager.explosionState.HurtsAll)
                col.gameObject.GetComponent<EnemyHealth>().damageTaken(dmg, transform.position);
        }
    }

    public void enableColliding(float radius, float d, float knockbcak, ExplosionManager.explosionState s) {
        state = s;
        dmg = d;
        maxKnockback = knockbcak;
        expCol.radius = radius;
        expCol.enabled = true;
    }
}
