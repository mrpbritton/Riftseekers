using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInstance : MonoBehaviour {
    Attack parentAttack;

    RocketBlast rb;

    bool exploded = false;

    private void Start() {
        rb = GetComponentInChildren<RocketBlast>();
    }

    private void OnCollisionEnter(Collision col) {
        switch(LayerMask.LayerToName(col.gameObject.layer)) {
            case "Enemy":
                //case "Other tag that would cause this to explode here":
                col.gameObject.GetComponent<EnemyHealth>().damageTaken(parentAttack.getRealDamage());
                rb.explode();
                break;
        }
    }

    public void setParentAttack(Attack at, float maxTimeAlive) {
        parentAttack = at;
        Invoke("forceExplosion", maxTimeAlive);
    }
    public void forceExplosion() {
        if(exploded)
            return;
        exploded = true;
        rb.explode();
    }
}
