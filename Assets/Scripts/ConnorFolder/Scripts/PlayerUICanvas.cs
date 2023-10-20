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

    public void updateSlider(int maxVal, float curVal) {
        slider.maxValue = maxVal;
        slider.value = curVal;
    }
}
