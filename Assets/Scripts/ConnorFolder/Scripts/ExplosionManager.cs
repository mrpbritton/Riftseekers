using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionManager : MonoBehaviour {
    //  None - doesn't hurt anyone
    public enum explosionState {
        None, HurtsEnemies, HurtsPlayer, HurtsAll
    }

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float holdTime, disappearTime, growTime;

    [SerializeField] bool smoothScale = false;

    private void Start() {
        DOTween.Init();
    }

    public void explode(Vector3 pos, float scale, explosionState state) {
        var obj = Instantiate(explosionPrefab.gameObject, pos, Quaternion.identity, null);
        obj.transform.position = pos;
        if(!smoothScale)
            obj.transform.localScale = new Vector3(scale, scale, scale);
        else {
            obj.transform.DOScale(scale, growTime);
            StartCoroutine(endExplosion(obj.transform));
        }
    }

    IEnumerator endExplosion(Transform obj) {
        yield return new WaitForSeconds(holdTime);
        obj.DOScale(0f, disappearTime);
        Destroy(obj.gameObject, disappearTime + .01f);
    }
}
