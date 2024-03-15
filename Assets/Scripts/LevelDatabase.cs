using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    public static LevelData currentLevelData = null;
    public static int currentLevel = 0;
    public static int latestLevel = 0;
    public List<LevelData> levels = new List<LevelData>();
    public void ChooseLevel(int num)
    {
        currentLevelData = levels[num];
        currentLevel = num;
    }
    public static void UpdateLatestLevel()
    {
        if(currentLevel >= latestLevel)
        {
            latestLevel = currentLevel + 1;
        }
    }
}
