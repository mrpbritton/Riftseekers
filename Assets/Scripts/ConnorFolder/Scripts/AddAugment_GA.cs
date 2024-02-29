using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAugment_GA : GameAction {
    [SerializeField] Augment_SO.augmentType refType;

    public override void Action() {
        FindObjectOfType<PlayerStats>().getAugmented(refType);
    }

    public override void DeAction() {
    }

    public override void ResetToDefault() {
    }
}
