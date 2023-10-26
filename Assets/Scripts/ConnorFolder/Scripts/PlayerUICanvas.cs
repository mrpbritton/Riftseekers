using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUICanvas : MonoBehaviour {
    [SerializeField, Tooltip("Slider this script accesses")]
    Slider slider;

    private void Start() {
        DOTween.Init();
        updateSlider(FindObjectOfType<CharacterFrame>().maxHealth, FindObjectOfType<CharacterFrame>().health);
    }

    public void updateSlider(float maxVal, float curVal) {
        slider.maxValue = maxVal;
        slider.value = curVal;
    }
    public void updateSlider(float cooldownTime) {
        slider.maxValue = 1;
        slider.value = 0;
        slider.DOValue(1, cooldownTime);
    }
}
