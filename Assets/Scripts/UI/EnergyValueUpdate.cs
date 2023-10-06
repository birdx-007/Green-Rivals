using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyValueUpdate : MonoBehaviour
{
    private Text text;
    public Slider slider;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = ManageSystem.instance.energy.ToString();
        slider.value = ManageSystem.instance.energy;
    }
}
