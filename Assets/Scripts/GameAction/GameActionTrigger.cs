using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionTrigger : GameAction
{
    [SerializeField]
    private List<GameAction> gAction;
    private bool bActive;
    [SerializeField]
    private LayerMask enemyMask;

    //Physics driven event that requires at least one object to have a rigid body
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != enemyMask) return;
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

    public override void Action()
    {
        if (bActive) return;

        StartCoroutine(nameof(DelayActions));
    }

    public void Play()
    {
        if (bActive) return;
        StartCoroutine(nameof(DelayActions));
    }

    public override void DeAction()
    {
        //nothing
    }
    public override void ResetToDefault()
    {
        //nothing
    }
}
