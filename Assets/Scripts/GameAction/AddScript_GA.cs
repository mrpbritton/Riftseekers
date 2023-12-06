using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddScript_GA : GameAction
{
    [SerializeField]
    private AttackType aType;

    public static Action<AttackType> ChangeAttackType = delegate { };
    public override void Action()
    {
        ChangeAttackType(aType);
    }

    public override void DeAction()
    {
     
    }

    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}