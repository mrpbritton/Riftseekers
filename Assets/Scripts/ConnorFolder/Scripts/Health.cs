using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
    [SerializeField, Tooltip("Any other script that changes values in this script will be shown if this is true")]
    private bool debugMode;

    public float MaxHealth => PlayerStats.MaxHealth;
    public float CurrentHealth => PlayerStats.Health;

    string playerHealthTag = "PlayerHealth";

    [SerializeField, Tooltip("Health slider that will be used to represent health")]
    PlayerUICanvas healthSlider;

    private void Start() 
    {
        //  saves if this is on the player object
        SaveData.setFloat(playerHealthTag, CurrentHealth);
        PlayerStats.UpdateHealth = SaveData.getInt(playerHealthTag, -1) == -1 ? CurrentHealth : SaveData.getInt(playerHealthTag);
        healthSlider.updateSlider(MaxHealth, CurrentHealth);
    }

    //  use this when taking damage
    public void takeDamage(float dmg) {
        PlayerStats.UpdateHealth -= dmg;

        healthSlider.updateSlider(MaxHealth, CurrentHealth);
        //  check if dead
        if(CurrentHealth <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        SaveData.setFloat(playerHealthTag, CurrentHealth);

        AkSoundEngine.PostEvent("PLayer_Hit", gameObject);


    }

    //  use this when healing
    public void heal(float dmg) {
        PlayerStats.UpdateHealth = Mathf.Clamp(CurrentHealth + dmg, 0, MaxHealth);

        healthSlider.updateSlider(MaxHealth, (int)CurrentHealth);
        //  check if dead (just in case)
        if(CurrentHealth <= 0)
            Debug.Log(gameObject.name + " died!");

        //  saves
        SaveData.setFloat(playerHealthTag, CurrentHealth);

        AkSoundEngine.PostEvent("Health_Pickup", gameObject);
    }

}
