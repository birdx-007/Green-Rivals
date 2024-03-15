using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    public static LevelData currentLevelData = null;
    public List<LevelData> levels = new List<LevelData>();
    public void ChooseLevel(int num)
    {
        currentLevelData = levels[num];
    }
}
