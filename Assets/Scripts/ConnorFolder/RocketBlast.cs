using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketBlast : MonoBehaviour {
    [SerializeField] float maxRadius;
    [SerializeField] float explodeTime;

    [SerializeField] float dmg;


    private void OnTriggerEnter(Collider col) {
        if(LayerMask.LayerToName(col.gameObject.layer) == "Enemy") {
            Debug.Log("Enemy exploded");
            col.gameObject.GetComponent<EnemyHealth>().damageTaken(dmg);
        }
    }

    private void Start() {
        transform.localScale = Vector3.zero;
    }

    public void explode() {
        var p = transform.parent.gameObject;
        transform.parent = null;
        Destroy(p.gameObject);
        transform.DOScale(new Vector3(maxRadius, maxRadius, maxRadius), explodeTime);
        Destroy(gameObject, explodeTime);
    }
}
