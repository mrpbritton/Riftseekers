using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject_GA : GameAction
{
    public override void Action()
    {
        Destroy(this.gameObject);
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
