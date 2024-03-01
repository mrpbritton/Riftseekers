using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddScript_GA : GameAction
{
    [Tooltip("Script that would be added")]
    public AttackScript ability;

    public override void Action()
    {
        AttackManager.I.ReplaceAttack(ability);    
    }

    public override void DeAction()
    {
     
    }

    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}