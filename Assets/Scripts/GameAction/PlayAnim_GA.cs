using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim_GA : GameAction
{
    [SerializeField, Tooltip("Animation that will be played")]
    private Animator anim;
    [SerializeField]
    private string actionID, deactionID;

    public override void Action()
    {
        anim.SetTrigger(actionID);
    }
    public override void DeAction()
    {
        anim.SetTrigger(deactionID);
    }
    public override void ResetToDefault()
    {
        //nothing
    }
}
