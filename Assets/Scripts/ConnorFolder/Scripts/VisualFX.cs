using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualFX : MonoBehaviour {
    [SerializeField] GameObject enemyCorpse;


    private void OnEnable() {
        EnemyHealth.onEnemyDeath += enemyDeathFX;
    }

    private void OnDisable() {
        EnemyHealth.onEnemyDeath -= enemyDeathFX;
    }

    public void enemyDeathFX(GameObject obj) {
        var peeta = Instantiate(enemyCorpse);

        GetComponent<CorpseManager>().addCorpse(peeta);
        peeta.transform.position = obj.transform.position;
    }
}
