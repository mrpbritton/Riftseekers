using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionTrigger : MonoBehaviour
{
    [SerializeField]
    private List<GameAction> gAction;
    private bool bActive;

    //Physics driven event that requires at least one object to have a rigid body
    private void OnTriggerEnter(Collider other)
    {
        if (bActive) return;
        StartCoroutine(nameof(DelayActions));
    }
    IEnumerator DelayActions()
    {
        bActive = true;
        foreach (GameAction item in gAction)
        {
            yield return new WaitForSeconds(item.delay);
            item.Action();
        }
        bActive = false;
    }
}
