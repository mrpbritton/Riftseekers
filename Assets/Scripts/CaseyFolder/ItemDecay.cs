using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDecay : MonoBehaviour
{
    Coroutine lifeSpan = null;
    [SerializeField]
    private int lifespan = 25;

    float shrinkTime = .25f;

    private void OnEnable()
    {
        lifeSpan = StartCoroutine(deathTimer());
    }

    private void OnDestroy()
    {
        StopCoroutine(deathTimer());
    }

    IEnumerator deathTimer()
    {
        yield return new WaitForSeconds(lifespan - shrinkTime);
        transform.DOScale(0f, shrinkTime);
        yield return new WaitForSeconds(shrinkTime);
        Destroy(gameObject);
    }
}
