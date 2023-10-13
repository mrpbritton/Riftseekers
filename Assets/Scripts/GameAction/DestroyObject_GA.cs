using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject_GA : GameAction
{
    public override void Action()
    {
        Debug.Log("die");
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
