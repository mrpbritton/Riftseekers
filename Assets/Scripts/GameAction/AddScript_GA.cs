using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddScript_GA : GameAction
{
    [SerializeField]
    private AttackScript aType;

    public static Action<AttackScript> ChangeAttackType = delegate { };
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