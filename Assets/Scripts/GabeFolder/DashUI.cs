using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashUI : MonoBehaviour
{
    public TMP_Text text;
    
    public void UpdateDashUI(int currentCharges)
    {
        text.text = currentCharges.ToString();
    }
}
