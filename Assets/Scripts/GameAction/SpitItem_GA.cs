using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Spits the item currently applied")]
public class SpitItem_GA : GameAction
{
    [Tooltip("ConItem that provides information to be updated")]
    public ConItem item;
    [Tooltip("Script that would be added")]
    public AttackScript ability;
    [SerializeField, Tooltip("Strength of the force")]
    private float forceStrength;

    private void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = AugmentLibrary.I.FindItem(ability).image;
        GetComponentInChildren<SpriteRenderer>().material = AugmentLibrary.I.getItemRarityMat(ability);
        transform.GetChild(0).LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public override void Action()
    {
        Transform player = AttackManager.I.transform;
        //Removes item saving function ---------
        Inventory.loadInventory();
        foreach(var i in Inventory.getItems(AugmentLibrary.I)) {
            if(i.overrideAbil == item.overrideAbil)
                Inventory.removeItem(Inventory.getItemIndex(i));
        }
        
        //The line below is messy, so let me explain it.
        #region Explanation
        /* This will set the Augment Library's item drop to what is currently applied.
         * however, to do this, we need to know what slot is being changed, so...

            var theAttackTypeWeNeed = AttackManager.I.GetAttackType(ability);

         * then, what we need to do, is get the script that is currently in there
         
            var currentAttackScript = AttackManager.I.GetAttackScript(theAttackTypeWeNeed);

         * lastly, put that in the ItemDrop
         
            Augment.I.SetItemDrop(attackScript);

         * What this all does is automatically find what slot needs to replaced, gets what is currently in there, then spits it out.
         */
        #endregion
        AugmentLibrary.I.SetItemDrop(AttackManager.I.GetAttackScript(AttackManager.I.GetAttackType(ability)));
        
        //Prepating the item --------
        GameObject go = Instantiate(AugmentLibrary.I.GetItemDrop(), player.position, Quaternion.identity);
        go.SetActive(true);
        Vector3 spitDir = (transform.position - player.position);
        spitDir = new Vector3(spitDir.x, 0, spitDir.z);
        
        //Spitting item ---------
        go.GetComponent<Rigidbody>().AddForce(spitDir * forceStrength, ForceMode.Impulse);
        AttackManager.I.ReplaceAttack(ability);
        
        //Saving item ---------
        Inventory.addItem(item);
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
