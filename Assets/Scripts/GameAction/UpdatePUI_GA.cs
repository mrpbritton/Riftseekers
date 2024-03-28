using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePUI_GA : GameAction
{
    [Tooltip("Item that will be used to change PUI")]
    public ConItem item;

    public override void Action()
    {
        switch (item.overrideAbil)
        {
            case Attack.AttackType.Melee:
                UIManager.I.puiCanvases[0].UpdateImage(item.image);
                break;
            case Attack.AttackType.Ranged:
                UIManager.I.puiCanvases[1].UpdateImage(item.image);
                break;
            case Attack.AttackType.Special:
                UIManager.I.puiCanvases[2].UpdateImage(item.image);
                break;
            case Attack.AttackType.Movement:
                UIManager.I.puiCanvases[3].UpdateImage(item.image);
                break;
            default:
                break;
        }
    }

    public override void DeAction()
    {

    }
    public override void ResetToDefault()
    {

    }
}
