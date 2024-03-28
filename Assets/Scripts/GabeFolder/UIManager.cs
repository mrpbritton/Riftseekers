using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Player UI Canvases (DO NOT CHANGE)")]
    public List<PlayerUICanvas> puiCanvases;

    private void OnEnable()
    {
        puiCanvases[0].UpdateImage(AttackManager.I.meleeAttack);
        puiCanvases[1].UpdateImage(AttackManager.I.rangedAttack);
        puiCanvases[2].UpdateImage(AttackManager.I.specialAttack);
        puiCanvases[3].UpdateImage(AttackManager.I.movementAttack);
    }

    public void ActivatePUI(Attack attack)
    {
        switch (attack.AType)
        {
            case Attack.AttackType.Melee: //sword
                puiCanvases[0].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Ranged: //gun
                puiCanvases[1].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Special: //rocket
                puiCanvases[2].updateSlider(attack.GetCooldownTime());
                break;
            case Attack.AttackType.Movement: //dash
                puiCanvases[3].updateSlider(attack.GetCooldownTime());
                break;
            default:
                Debug.Log($"aType of attack is not melee, ranged, special, or movement. Check that it isn't \"None\".");
                break;
        }
    }
}
