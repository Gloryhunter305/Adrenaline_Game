using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStaminaBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

}