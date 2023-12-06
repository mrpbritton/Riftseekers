using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseManager : MonoBehaviour {
    KdTree<Transform> corpses = new KdTree<Transform>();
    [SerializeField] float minDistFromPlayer, pushSpeed;

    Transform playerTrans, closest, prevClosest;



    private void Start() {
        playerTrans = FindObjectOfType<PlayerMovement>().transform;
    }

    private void LateUpdate() {
        if(corpses.Count == 0) return;
        closest = corpses.FindClosest(playerTrans.position);

        //  changed closests so swap active colliders
        if(prevClosest != closest) {
            //prevClosest.GetComponent<Collider>().enabled = false;
            //closest.GetComponent<Collider>().enabled = true;
        }
        float dist = Vector2.Distance(new Vector2(playerTrans.position.x, playerTrans.position.z), new Vector2(closest.position.x, closest.position.z));

        //  get pushed
        if(dist < minDistFromPlayer) {
            closest.transform.position = Vector3.MoveTowards(closest.transform.position, new Vector3(playerTrans.position.x, closest.transform.position.y, playerTrans.position.z), -pushSpeed * Time.deltaTime);
        }

        prevClosest = closest;
    }

    public void addCorpse(GameObject c) {
        //c.GetComponent<Collider>().enabled = corpses.Count == 0;
        corpses.Add(c.transform);
    }
    public void removeCorpse(GameObject c) {
        corpses.RemoveAll(x => x.gameObject == c.gameObject);
    }
}
