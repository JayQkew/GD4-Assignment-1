using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Update() {
        slider.maxValue = GetComponent<PlayerStats>().GetStatValue(StatName.Fuel);
        slider.value = GetComponent<Movement>().currFuel;
    }
}
