using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class VisualFX : MonoBehaviour {
    [SerializeField] GameObject enemyCorpse, hitParticles;
    [SerializeField] GameObject bPool;
    [SerializeField] float minPoolSize, maxPoolSize, poolTime, poolColorTime;
    [SerializeField] float poolEndAlpha;


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

    public void hitFX(Transform defender, Vector3 attackPoint) {
        var dir = defender.transform.position - attackPoint;
        var p = Instantiate(hitParticles);
        p.transform.position = defender.position;
        dir.Normalize();
        p.transform.rotation = Quaternion.LookRotation(dir);
    }
    public void enemyDeathFX(GameObject obj) {
        //  corpse
        var peeta = Instantiate(enemyCorpse);
        GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = obj.transform.position;

        //  blood pool
        var p = Instantiate(bPool);
        float dpDepth = .1f;
        p.transform.position = peeta.transform.position + new Vector3(0f, -peeta.transform.lossyScale.y - dpDepth / 2f, 0f);
        var dp = p.GetComponent<DecalProjector>();
        dp.size = Vector3.zero;

        //  scales
        float curSize = 0f;
        float targetSize = Random.Range(minPoolSize, maxPoolSize);
        DOTween.To(() => curSize, x => curSize = x, targetSize, poolTime).OnUpdate(() => {
            dp.size = new Vector3(curSize, curSize, dpDepth);
        });
        //  colors
        float curColor = 1f;
        DOTween.To(() => curColor, x => curColor = x, poolEndAlpha, poolColorTime).OnUpdate(() => {
            dp.fadeFactor = curColor;
        });
    }
}
