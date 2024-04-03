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

    List<GameObject> corpsePool = new List<GameObject>();
    List<GameObject> poolPool = new List<GameObject>();
    int corpsePoolCount = 300;

    private void Awake() {
        DOTween.Init();

        lowest = FindObjectOfType<PlayerMovement>().transform.position.y;
        lowest -= FindObjectOfType<PlayerMovement>().transform.lossyScale.y / 6f;
    }

    private void Start() {
        for(int i = 0; i < corpsePoolCount; i++) {
            var temp = Instantiate(enemyCorpse);
            temp.SetActive(false);
            corpsePool.Add(temp);

            var tempP = Instantiate(bPool);
            tempP.SetActive(false);
            poolPool.Add(tempP);
        }
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
        var c = getCorpse();
        //GetComponent<CorpseManager>().addCorpse(peeta);
        c.transform.position = obj.position;
        StartCoroutine(corpseClear(c.transform));

        //  blood pool
        var p = getPool();
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

    GameObject getCorpse() {
        var temp = corpsePool[0];
        temp.SetActive(true);
        corpsePool.RemoveAt(0);
        temp.transform.localScale = Vector3.one * .75f;
        return temp;
    }
    GameObject getPool() {
        var temp = poolPool[0];
        temp.SetActive(true);
        poolPool.RemoveAt(0);
        temp.transform.localScale = Vector3.one;
        return temp;
    }

    IEnumerator corpseClear(Transform corpse) {
        yield return new WaitForSeconds(2f);
        corpse.DOScale(0f, .25f);
        yield return new WaitForSeconds(.26f);
        corpse.gameObject.SetActive(false);
        corpsePool.Add(corpse.gameObject);
    }

    IEnumerator bloodPoolClear(Transform b) {
        yield return new WaitForSeconds(2.5f);
        var dp = b.GetComponent<DecalProjector>();
        float curSize = dp.size.x;
        float targetSize = Random.Range(minPoolSize, maxPoolSize);
        DOTween.To(() => curSize, x => curSize = x, 0f, .25f).OnUpdate(() => {
            dp.size = new Vector3(curSize, curSize, .2f);
        });
        yield return new WaitForSeconds(.26f);
        b.gameObject.SetActive(false);
        poolPool.Add(b.gameObject);
    }
}
