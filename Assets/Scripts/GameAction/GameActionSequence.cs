using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionSequence : GameAction
{
    [SerializeField]
    private List<GameAction> actionList;

    public override void Action()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(nameof(Sequence));
    }

    IEnumerator Sequence()
    {
        foreach (GameAction item in actionList)
        {
            yield return new WaitForSeconds(item.delay);
            item.Action();
        }
    }
}
