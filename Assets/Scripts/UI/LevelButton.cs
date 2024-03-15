using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNum;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = levelNum <= LevelDatabase.latestLevel;
    }
}
