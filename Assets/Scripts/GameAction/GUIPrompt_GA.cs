using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPrompt_GA : GameAction
{
    public override void Action()
    {
        Debug.Log("User prompted!");
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
