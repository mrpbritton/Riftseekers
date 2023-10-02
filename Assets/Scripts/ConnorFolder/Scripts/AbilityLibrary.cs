using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLibrary : MonoBehaviour {
    public enum abilType {
        None, Rocket, Dash, Sword, Pistol
    }

    [SerializeField] List<Attack> abs = new List<Attack>();
    [SerializeField] Dictionary<abilType, Attack> abils = new Dictionary<abilType, Attack>();


    private void Awake() {
        if(abs.Count == 0)
            Debug.LogError("Add Player Abilities to the Ability Library");
        else if(abs[0].abilType == abilType.None)
            Debug.LogError("Assign abilType to attack scripts");
        foreach(var i in abs)
            abils.Add(i.abilType, i);
    }

    public Attack getAttack(abilType type) {
        if(type == abilType.None)
            return null;
        return abils[type];
    }
}
