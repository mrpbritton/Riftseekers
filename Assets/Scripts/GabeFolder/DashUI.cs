using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashUI : MonoBehaviour
{
    private static TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public static void UpdateDashUI(int currentCharges)
    {
        text.text = currentCharges.ToString();
    }
}
