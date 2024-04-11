using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLore_GA : GameAction
{
    [Tooltip("LoreItem that provides information to be updated")]
    public LorePiece_SO lore;

    private void Start()
    {
    }

    public override void Action()
    {
        //Add Lore to save stuff
        AugmentLibrary.I.RemoveLore(lore);
        //Save lore stuff
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
