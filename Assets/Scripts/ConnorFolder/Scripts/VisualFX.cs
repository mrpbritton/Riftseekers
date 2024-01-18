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
        //GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = obj.transform.position;
        StartCoroutine(corpseClear(peeta.transform));

        //  blood pool
        var p = Instantiate(bPool);
        p.transform.position = peeta.transform.position + new Vector3(0f, -peeta.transform.lossyScale.y + .2f, 0f);
        var dp = p.GetComponent<DecalProjector>();
        dp.size = Vector3.zero;
        StartCoroutine(bloodPoolClear(p.transform));

        //  scales
        float curSize = 0f;
        float targetSize = Random.Range(minPoolSize, maxPoolSize);
        DOTween.To(() => curSize, x => curSize = x, targetSize, poolTime).OnUpdate(() => {
            dp.size = new Vector3(curSize, curSize, .2f);
        });
        //  colors
        float curColor = 1f;
        DOTween.To(() => curColor, x => curColor = x, poolEndAlpha, poolColorTime).OnUpdate(() => {
            dp.fadeFactor = curColor;
        });
    }

    IEnumerator corpseClear(Transform corpse) {
        yield return new WaitForSeconds(10f);
        corpse.DOScale(0f, .25f);
        Destroy(corpse.gameObject, .26f);
    }

    IEnumerator bloodPoolClear(Transform b) {
        yield return new WaitForSeconds(10.5f);
        var dp = b.GetComponent<DecalProjector>();
        float curSize = dp.size.x;
        float targetSize = Random.Range(minPoolSize, maxPoolSize);
        DOTween.To(() => curSize, x => curSize = x, 0f, .25f).OnUpdate(() => {
            dp.size = new Vector3(curSize, curSize, .2f);
        });
        Destroy(b.gameObject, .26f);
    }
}
