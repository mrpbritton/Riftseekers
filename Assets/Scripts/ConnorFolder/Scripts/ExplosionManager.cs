using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionManager : Singleton<ExplosionManager> {
    //  None - doesn't hurt anyone
    public enum explosionState {
        None, HurtsEnemies, HurtsPlayer, HurtsAll
    }

    [SerializeField] GameObject redExplosionPref, blueExplosionPref;
    [SerializeField] float holdTime, disappearTime, growTime;

    private void Start() {
        DOTween.Init();
    }

    public void explode(Vector3 pos, float scale, float dmg, float knockback, explosionState state) {
        var obj = Instantiate(redExplosionPref.gameObject, pos, Quaternion.identity, null);
        obj.GetComponent<ExplosionCollider>().enableColliding(scale, dmg, knockback, state);

        //  pos / scale
        obj.transform.position = pos;
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void blueExplode(Vector3 pos, float scale, float dmg, float knockback, explosionState state) {
        var obj = Instantiate(blueExplosionPref.gameObject, pos, Quaternion.identity, null);
        obj.GetComponent<ExplosionCollider>().enableColliding(scale, dmg, knockback, state);

        //  pos / scale
        obj.transform.position = pos;
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }

    //  returns the index of the started queue in the queue list (so that we can stop them if we need to)
    public Coroutine queueExplode(Transform pos, float scale, float dmg, float knockback, explosionState state, float delay) {
        return StartCoroutine(queueExplodeWaiter(pos, scale, dmg, knockback, state, delay));
    }

    IEnumerator endExplosion(Transform obj) {
        yield return new WaitForSeconds(holdTime);
        obj.DOScale(0f, disappearTime);
        Destroy(obj.gameObject, disappearTime + .01f);
    }
    IEnumerator queueExplodeWaiter(Transform pos, float scale, float dmg, float knockback, explosionState state, float delay) {
        yield return new WaitForSeconds(delay);
        if(pos != null)
            explode(pos.position, scale, dmg, knockback, state);
    }
}
