using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLibrary : MonoBehaviour {

    [SerializeField] List<Attack> abs = new List<Attack>();
    [SerializeField] Dictionary<Attack.attackType, Attack> abils = new Dictionary<Attack.attackType, Attack>();

    private void Awake() {
        if(abs.Count == 0)
            Debug.LogError("Add Player Abilities to the Ability Library");
        else if(abs[0].aType == Attack.attackType.None)
            Debug.LogError("Assign abilType to attack scripts");
        foreach(var i in abs)
            abils.Add(i.aType, i);
    }

    public Attack getAttack(Attack.attackType type) {
        if(type == Attack.attackType.None)
            return null;
        return abils[type];
    }
}
