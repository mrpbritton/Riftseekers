using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class ExplosionManager : MonoBehaviour {
    //  None - doesn't hurt anyone
    public enum explosionState {
        None, HurtsEnemies, HurtsPlayer, HurtsAll
    }

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float holdTime, disappearTime, growTime;

    [SerializeField] bool smoothScale = false;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.H))
            explode(FindObjectOfType<PlayerManager>().transform.position + new Vector3(3f, 0f, 3f), 5f, explosionState.None);
    }

    private void Start() {
        DOTween.Init();
    }

    public void explode(Vector2 pos, float scale, explosionState state) {
        var obj = Instantiate(explosionPrefab.gameObject, pos, Quaternion.identity, null);
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
