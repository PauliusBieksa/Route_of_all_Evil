using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashDisplayer : MonoBehaviour
{
    public TextMeshProUGUI cashText;
    public GameState state;
    public float updateSpeed = 1;
    
    void Update()
    {
        cashText.text = $"£{state.cash:0.##}";
    }

}
