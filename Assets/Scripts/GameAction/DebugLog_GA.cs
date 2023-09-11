using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLog_GA : GameAction
{
    [SerializeField, Tooltip("What the Debug.Log will say")]
    private string message; 
    public override void Action()
    {
        Debug.Log(message);
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
