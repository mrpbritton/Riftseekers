using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLibrary : MonoBehaviour {

    [SerializeField] List<Attack> abs = new List<Attack>();
    [SerializeField] Dictionary<Attack.AttackType, Attack> abils = new Dictionary<Attack.AttackType, Attack>();

    private void Awake() {
        if(abs.Count == 0)
            Debug.LogError("Add Player Abilities to the Ability Library");
        else if(abs[0].AType == Attack.AttackType.None)
            Debug.LogError("Assign abilType to attack scripts");
        foreach(var i in abs)
            abils.Add(i.AType, i);
    }

    public Attack getAttack(Attack.AttackType type) {
        if(type == Attack.AttackType.None)
            return null;
        return abils[type];
    }
}
