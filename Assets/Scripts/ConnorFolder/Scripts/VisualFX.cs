using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisualFX : MonoBehaviour {
    [SerializeField] GameObject enemyCorpse;
    [SerializeField] GameObject bPool;
    [SerializeField] float minBPoolSize, maxBPoolSize, bPoolTime, bShowTime;
    [SerializeField] Color bPoolEndColor;


    private void Start() {
        DOTween.Init();
    }

    public void enemyDeathFX(GameObject obj) {
        //  corpse
        var peeta = Instantiate(enemyCorpse);
        GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = obj.transform.position;

        //  blood pool
        var p = Instantiate(bPool);
        p.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - obj.transform.lossyScale.y / 2f + .01f, obj.transform.position.z);
        p.transform.localScale = Vector3.zero;
        float size = Random.Range(minBPoolSize, maxBPoolSize);
        p.transform.DOScale(size, bPoolTime);
        p.GetComponent<SpriteRenderer>().DOColor(bPoolEndColor, bShowTime + bPoolTime);
        if(bPoolEndColor.a == 0f)
            Destroy(p.gameObject, bShowTime + bPoolTime + .01f);
    }
}
