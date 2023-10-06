using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundValueUpdate : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = ManageSystem.instance.steps.ToString();
    }
}
