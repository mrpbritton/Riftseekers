using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvas : MonoBehaviour {
    [SerializeField, Tooltip("Slider this script accesses")] 
    Slider slider;

    private void Start() {
        if(slider != null) {
            updateSlider(FindObjectOfType<CharacterFrame>().maxHealth, FindObjectOfType<CharacterFrame>().health);
        }
    }

    public void updateSlider(float maxVal, float curVal) {
        slider.maxValue = maxVal;
        slider.value = curVal;
    }
    public void updateSlider(float cooldownTime)
    {
        slider.maxValue = cooldownTime;
        slider.value = 0;
        StartCoroutine(UpdateCooldownBar(cooldownTime));
    }

    public IEnumerator UpdateCooldownBar(float cooldownTime)
    {
        float dTimeRemaining = cooldownTime;
        float maxTime = slider.maxValue;
        /* The loop below ends up being a pseudo-update function. This is able to 
         * happen because of the yield return null; at the end of this while loop.
         * Every iteration makes the dTimeRemaining decrease until it is zero (or 
         * until the dash is completed), while the position of the player continues 
         * to move towards the direction */

        while (dTimeRemaining > 0)
        {
            dTimeRemaining -= Time.deltaTime;
            slider.value = (maxTime - dTimeRemaining)/maxTime;
            //Debug.Log("AAA");
            yield return null;
        }

        yield return null;
        //since dash was performed, subtract a dash
    }
}
