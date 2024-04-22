using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDecay : MonoBehaviour
{
    Coroutine lifeSpan = null;
    [SerializeField]
    private int lifespan = 25;

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
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
