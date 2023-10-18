using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisualFX : MonoBehaviour {
    [SerializeField] GameObject enemyCorpse, hitParticles;
    [SerializeField] GameObject bPool;
    [SerializeField] float minPoolSize, maxPoolSize, poolTime, poolColorTime;
    [SerializeField] Color poolStartColor, poolEndColor;


    private void Awake() {
        DOTween.Init();
    }

    private void OnEnable() {
        EnemyHealth.onEnemyDeath += enemyDeathFX;
        EnemyHealth.onEnemyHit += hitFX;
    }

    private void OnDisable() {
        EnemyHealth.onEnemyDeath -= enemyDeathFX;
        EnemyHealth.onEnemyHit -= hitFX;
    }

    public void hitFX(Transform defender, Vector2 attackPoint) {
        var dir = (Vector2)defender.transform.position - attackPoint;
        var p = Instantiate(hitParticles);
        p.transform.position = defender.position;
        p.transform.LookAt(attackPoint, Vector3.up);
    }
    public void enemyDeathFX(GameObject obj) {
        //  corpse
        var peeta = Instantiate(enemyCorpse);
        GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = obj.transform.position;

        //  blood pool
        var p = Instantiate(bPool);
        var yP = peeta.transform.position.y - peeta.transform.lossyScale.y / 2.0f;
        p.transform.position = new Vector3(peeta.transform.position.x, yP + .1f, peeta.transform.position.z);
        p.transform.localScale = Vector3.zero;
        p.transform.DOScale(Random.Range(minPoolSize, maxPoolSize), poolTime);
        p.GetComponent<SpriteRenderer>().color = poolStartColor;
        p.GetComponent<SpriteRenderer>().DOColor(poolEndColor, poolColorTime);
    }
}
