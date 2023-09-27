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

    PlayerUICanvas pui;

    public void UpdateStats()
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
        pui = FindObjectOfType<PlayerUICanvas>();
        pui.updateHealthSlider(maxHealth, (int)health);
        isPlayer = gameObject.tag == "Player";
        health = isPlayer ? SaveData.getInt(playerHealthTag) : maxHealth;

        //  saves if this is on the player object
        if(isPlayer)
        {
            SaveData.setFloat(playerHealthTag, health);
            frame = GetComponent<CharacterFrame>();
        }
            
    }

    //  use this when taking damage
    public void takeDamage(int dmg) {
        health -= dmg;

        pui.updateHealthSlider(maxHealth, (int)health);
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

        pui.updateHealthSlider(maxHealth, (int)health);
        //  check if dead (just in case)
        if(health <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        if(isPlayer)
            SaveData.setFloat(playerHealthTag, health);
    }
}
