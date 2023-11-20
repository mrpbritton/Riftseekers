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

    //  returns the index of the started queue in the queue list (so that we can stop them if we need to)
    public Coroutine queueExplode(Transform pos, float scale, explosionState state, float delay) {
        return StartCoroutine(queueExplodeWaiter(pos, scale, state, delay));
    }

    IEnumerator endExplosion(Transform obj) {
        yield return new WaitForSeconds(holdTime);
        obj.DOScale(0f, disappearTime);
        Destroy(obj.gameObject, disappearTime + .01f);
    }
    IEnumerator queueExplodeWaiter(Transform pos, float scale, explosionState state, float delay) {
        yield return new WaitForSeconds(delay);
        if(pos != null)
            explode(pos.position, scale, state);
    }
}
