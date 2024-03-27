using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour 
{
    [SerializeField, Tooltip("Any other script that changes values in this script will be shown if this is true")]
    private bool debugMode;

    public float MaxHealth => PlayerStats.MaxHealth;
    public float CurrentHealth => PlayerStats.Health;

    [SerializeField] SpriteRenderer sr;

    string playerHealthTag = "PlayerHealth";

    [SerializeField, Tooltip("Health slider that will be used to represent health")]
    PlayerUICanvas healthSlider;

    Coroutine invincWaiter = null;

    private void Start() 
    {
        //  saves if this is on the player object
        SaveData.setFloat(playerHealthTag, CurrentHealth);
        PlayerStats.UpdateHealth = SaveData.getInt(playerHealthTag, -1) == -1 ? CurrentHealth : SaveData.getInt(playerHealthTag);
        healthSlider.updateSlider(MaxHealth, CurrentHealth);
    }

    //  use this when taking damage
    public void takeDamage(float dmg) {
        if(invincWaiter != null) return;
        invincWaiter = StartCoroutine(invincTimer(.5f));

        PlayerStats.UpdateHealth -= dmg;

        healthSlider.updateSlider(MaxHealth, CurrentHealth);
        //  check if dead
        if(CurrentHealth <= 0)
        {
            Debug.Log(gameObject.name + " died!");
            SceneManager.LoadScene("Death");
        }
            

        //  saves
        SaveData.setFloat(playerHealthTag, CurrentHealth);

        AkSoundEngine.PostEvent("PLayer_Hit", gameObject);


    }

    IEnumerator invincTimer(float t) {
        sr.color = new Color(1f, 0f, 0f, .5f);
        sr.DOColor(Color.white, t);
        yield return new WaitForSeconds(t);
        sr.color = Color.white;
        invincWaiter = null;
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
