using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
    [SerializeField] int maxHealth;
    public float health { get; private set; }
    private CharacterFrame frame;

    bool isPlayer;

    string playerHealthTag = "PlayerHealth";

    private void UpdateStats()
    {
        if(isPlayer)
        {
            maxHealth = frame.maxHealth;
            health = frame.health;
            health = Mathf.Clamp(health, 0, maxHealth);
            frame.health = health;
            if (health <= 0)
                Debug.Log(gameObject.name + " died!");
        }
    }

    private void Start() {
        isPlayer = gameObject.tag == "Player";
        health = isPlayer ? SaveData.getInt(playerHealthTag) : maxHealth;

        //  saves if this is on the player object
        if(isPlayer)
        {
            SaveData.setFloat(playerHealthTag, health);
            frame = GetComponent<CharacterFrame>();
            CharacterFrame.UpdateStats += UpdateStats;
        }
            
    }

    private void OnDestroy()
    {
        CharacterFrame.UpdateStats -= UpdateStats;
    }

    //  use this when taking damage
    public void takeDamage(int dmg) {
        health -= dmg;

        //  check if dead
        if(health <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        if(isPlayer)
            SaveData.setFloat(playerHealthTag, health);
    }

    //  use this when healing
    public void heal(int dmg) {
        health = Mathf.Clamp(health + dmg, 0, maxHealth);

        //  check if dead (just in case)
        if(health <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        if(isPlayer)
            SaveData.setFloat(playerHealthTag, health);
    }
}
