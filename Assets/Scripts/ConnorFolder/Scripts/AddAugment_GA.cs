using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAugment_GA : GameAction {
    [SerializeField] 
    public Augment_SO.augmentType refType;

    private void Start() {
        GetComponent<SpriteRenderer>().sprite = AugmentLibrary.I.getAugment(refType).sprite;
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public override void Action() {
        FindObjectOfType<PlayerStats>().getAugmented(refType);
        FindObjectOfType<PlayerStats>().save();
    }

    public override void DeAction() {
    }

    public override void ResetToDefault() {
    }
}
