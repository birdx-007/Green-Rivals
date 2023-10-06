using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyGenerater : MonoBehaviour
{
    public static EnemyGenerater instance;
    public List<GameObject> RecyclabeEnemies;
    public List<GameObject> HarmlessEnemies;
    public List<GameObject> RadiationEnemies;
    public List<GameObject> PollutionEnemies;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void GenerateEnemy(EnemyType type, int varition, int row, int col)
    {
        CellController cell = ChessboardController.instance.GetCell(row, col);
        Vector3 pos = ChessboardController.instance.GetCellPosition(row, col);
        //Player oldPlayer = cell.GetComponent<DraggableAcceptor>().acceptedObject.GetComponent<Player>();// omg 这行代码的出现说明让draggableAcceptor来间接动态存储Player实在太蠢了
        Player oldPlayer = cell.attachedPlayer;
        Enemy oldEnemy = cell.attachedEnemy;
        Enemy newEnemy;
        GameObject newObject = null;
        switch (type)
        {
            case EnemyType.Recyclable:
                if (varition >= 0 && (oldEnemy == null || oldEnemy.type != EnemyType.Radiation))
                {
                    newObject = Instantiate(RecyclabeEnemies[varition], pos, Quaternion.identity, transform);
                }

                break;
            case EnemyType.Harmless:
                if (varition >= 0 && (oldEnemy == null || oldEnemy.type != EnemyType.Radiation))
                {
                    newObject = Instantiate(HarmlessEnemies[varition], pos, Quaternion.identity, transform);
                }

                break;
            case EnemyType.Radiation:
                if (varition >= 0 && (oldPlayer==null || oldPlayer.type != PlayerType.Cleaner))
                {
                    newObject = Instantiate(RadiationEnemies[varition], pos, Quaternion.identity, transform);
                }

                break;
            case EnemyType.Pollution:
                if (varition >= 0 && (oldEnemy == null) && (oldPlayer == null || oldPlayer.type != PlayerType.Cleaner))
                {
                    newObject = Instantiate(PollutionEnemies[varition], pos, Quaternion.identity, transform);
                }
                break;
        }

        if (newObject != null)
        {
            if (oldEnemy != null)
            {
                oldEnemy.isAlive = false;
            }
            newEnemy = newObject.GetComponent<Enemy>();
            cell.attachedEnemy = newEnemy;
            newEnemy.Cell = cell;
            if (newEnemy.type == EnemyType.Radiation && oldPlayer != null && oldPlayer.type != PlayerType.Cleaner)
            {
                oldPlayer.isAlive = false;
            }
            ManageSystem.instance.leftEnemies++;
        }
    }
}
