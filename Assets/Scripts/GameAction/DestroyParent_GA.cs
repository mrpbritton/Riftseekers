using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent_GA : GameAction
{
    public override void Action()
    {
        Destroy(transform.parent.gameObject);
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
