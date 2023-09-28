using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvas : MonoBehaviour {
    [SerializeField] Slider slider;

    public void updateHealthSlider(int maxHp, int hp) {
        slider.maxValue = maxHp;
        slider.value = hp;
    }
}
