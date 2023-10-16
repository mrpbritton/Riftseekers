using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualFX : MonoBehaviour {
    [SerializeField] GameObject enemyCorpse;

    public void enemyDeathFX(Vector3 pos) {
        var peeta = Instantiate(enemyCorpse);

        GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = pos;
    }
}
