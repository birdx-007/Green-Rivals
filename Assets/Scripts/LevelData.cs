using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyInfo
{
    public EnemyType type;
    public int varition;
    public int row;
    public int col;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "GreenRivals/LevelData")]
public class LevelData : ScriptableObject
{
    public int energy;
    public int chessboardColNum = 8;
    public int chessboardRowNum = 5;
    public List<EnemyInfo> enemies;
}
