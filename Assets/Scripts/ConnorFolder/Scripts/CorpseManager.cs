using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseManager : MonoBehaviour {
    KdTree<Transform> corpses = new KdTree<Transform>();
    [SerializeField] float minDistFromPlayer, pushSpeed;

    Transform playerTrans, closest, prevClosest;



    private void Start() {
        this.enabled = false;
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
    }

    public void addCorpse(GameObject c) {
        //c.GetComponent<Collider>().enabled = corpses.Count == 0;
        corpses.Add(c.transform);
    }
    public void removeCorpse(GameObject c) {
        corpses.RemoveAll(x => x.gameObject == c.gameObject);
    }
}
