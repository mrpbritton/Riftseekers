using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
    [SerializeField] 
    public int maxHealth;
    public float health { get; private set; }
    private CharacterFrame frame;

    bool isPlayer;

    string playerHealthTag = "PlayerHealth";

    [SerializeField, Tooltip("Health slider that will be used to represent health")]
    PlayerUICanvas healthSlider;

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

    private void Start() 
    {
        isPlayer = gameObject.tag == "Player";

        //  saves if this is on the player object
        if(isPlayer)
        {
            SaveData.setFloat(playerHealthTag, health);
            frame = GetComponent<CharacterFrame>();
        }
        health = isPlayer ? SaveData.getInt(playerHealthTag, -1) == -1 ? frame.health : SaveData.getInt(playerHealthTag) : maxHealth;
        healthSlider.updateSlider(frame.maxHealth, (int)frame.health);
    }

    //  use this when taking damage
    public void takeDamage(float dmg) {
        health -= dmg;

        healthSlider.updateSlider(maxHealth, (int)health);
        //  check if dead
        if(health <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        if(isPlayer)
            SaveData.setFloat(playerHealthTag, health);

        AkSoundEngine.PostEvent("PLayer_Hit", gameObject);


    }

    //  use this when healing
    public void heal(float dmg) {
        health = Mathf.Clamp(health + dmg, 0, maxHealth);

        healthSlider.updateSlider(maxHealth, (int)health);
        //  check if dead (just in case)
        if(health <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        if(isPlayer)
            SaveData.setFloat(playerHealthTag, health);

        AkSoundEngine.PostEvent("Health_Pickup", gameObject);

    }

    private void OnEnable()
    {
        health = maxHealth;
    }
}
