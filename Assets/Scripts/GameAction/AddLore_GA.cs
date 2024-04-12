using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLore_GA : GameAction
{
    public override void Action()
    {
        AugmentLibrary.I.getAndRemoveLore();
        Inventory.saveInventory();
    }

    public override void DeAction()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}
