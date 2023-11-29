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

    private void Start() {
        DOTween.Init();
    }

    public void explode(Vector3 pos, float scale, explosionState state) {
        explodeWithColor(pos, scale, state, Color.white);
    }
    public void explodeWithColor(Vector3 pos, float scale, explosionState state, Color c) {
        var obj = Instantiate(explosionPrefab.gameObject, pos, Quaternion.identity, null);

        //  sets color if there is a custom color
        if(c != Color.white) {
            var m = obj.GetComponent<ParticleSystemRenderer>().material.color = c;
            foreach(var i in obj.GetComponentsInChildren<ParticleSystemRenderer>()) {
                i.material.color = c;
            }
        }

        //  pos / scale
        obj.transform.position = pos;
        obj.transform.localScale = new Vector3(scale, scale, scale);
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
