using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvas : MonoBehaviour {
    [SerializeField, Tooltip("Slider this script accesses")] 
    Slider slider;

    public void updateSlider(int maxVal, int curVal) {
        slider.maxValue = maxVal;
        slider.value = curVal;
    }
}
