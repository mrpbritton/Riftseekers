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
    float lowest;

    private void Awake() {
        DOTween.Init();

        lowest = FindObjectOfType<PlayerMovement>().transform.position.y;
        lowest -= FindObjectOfType<PlayerMovement>().transform.lossyScale.y / 6f;
    }

    private void OnEnable() {
        EnemyHealth.onEnemyDeath += enemyDeathFX;
        EnemyHealth.onEnemyHit += hitFX;
    }

    private void OnDisable() {
        EnemyHealth.onEnemyDeath -= enemyDeathFX;
        EnemyHealth.onEnemyHit -= hitFX;
    }

    public void hitFX(Transform defender, Vector3 attackPoint, float damage) {
        var dir = defender.transform.position - attackPoint;
        var p = Instantiate(hitParticles);
        p.transform.position = defender.position;
        dir.Normalize();
        p.transform.rotation = Quaternion.LookRotation(dir);
    }
    public void enemyDeathFX(Transform obj) {
        //  corpse
        var c = Instantiate(enemyCorpse);
        //GetComponent<CorpseManager>().addCorpse(peeta);
        c.transform.position = obj.position;
        StartCoroutine(corpseClear(c.transform));

        //  blood pool
        var p = Instantiate(bPool);
        p.transform.position = c.transform.position;
        p.transform.position = new Vector3(p.transform.position.x, lowest, p.transform.position.z);
        var dp = p.GetComponent<DecalProjector>();
        dp.size = new Vector3(0f, dp.size.y, dp.size.z);
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
