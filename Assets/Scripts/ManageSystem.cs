using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ManageSystem : MonoBehaviour
{
    public bool hasGenerate = false;
    public static ManageSystem instance;
    public float playerMoveTime = 0.5f; // 玩家前进一格所需时间
    public UnityEvent OnGameWin;
    public int leftEnemies = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
        players = new Dictionary<int, Player>();
        enemies = new Dictionary<int, Enemy>();
    }

    [NonSerialized] public int energy; // 剩余能量
    [NonSerialized] public int steps; // 玩家移动步数

    [NonSerialized] public bool isProcessing = false; // 回合已开始 各单位正在自主移动交互中
    [NonSerialized] public bool arePlayersMoving = false;

    //private List<Player> players;
    //private List<Enemy> enemies;
    public Dictionary<int, Player> players;
    public Dictionary<int, Enemy> enemies;

    private void Start()
    {
        Initialize(7);
    }

    void Initialize(int newEnergy)
    {
        energy = newEnergy;
        steps = 0;
        isProcessing = false;
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Destroy(pair.Value.gameObject);
        }

        players.Clear();
        foreach (KeyValuePair<int, Enemy> pair in enemies)
        {
            Destroy(pair.Value.gameObject);
        }
        enemies.Clear();
    }

    /// <summary>
    /// 开始回合 供玩家摆放完毕后自行执行
    /// </summary>
    public void StartRound()
    {
        isProcessing = true;
        steps++;
        MoveAllPlayersForward();
    }

    private void Update()
    {
        if (!hasGenerate)
        {
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Recyclable, 0, 1, 4);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Recyclable, 1, 4, 5);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Recyclable, 2, 5, 6);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Recyclable, 0, 2, 8);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Harmless,1,2,5);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Harmless,0,4,8);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Radiation, 0, 3, 4);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Radiation, 0, 2, 6);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Radiation, 0, 3, 5);
            EnemyGenerater.instance.GenerateEnemy(EnemyType.Radiation, 0, 3, 6);
            hasGenerate = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isProcessing)
        {
            StartRound();
        }
        if (isProcessing)
        {
            if (!arePlayersMoving)
            {
                UpdatePlayersAndEnemiesState();
                isProcessing = false;
                if (isWinning())
                {
                    OnGameWin.Invoke();
                }
            }
        }
    }

    private bool isWinning()
    {
        if (hasGenerate && (leftEnemies == 0||enemies.Count==0))
        {
            return true;
        }

        return false;
    }

    public void MoveAllPlayersForward()
    {
        CellController cell = null;
        CellController nextCell = null;
        // players detach former acceptor
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Player player = pair.Value;
            cell = player.Cell;
            nextCell = cell.GetNeighbor(CellController.Direction.Right);
            if (nextCell != null)
            {
                Debug.Log("player move from "+cell.transform.position+" to " +nextCell.transform.position);
                player.MoveToCell(nextCell, playerMoveTime);
            }
            else
            {
                Debug.Log("player out of field.");
                player.isAlive = false;
            }
        }
        // players attach new acceptor
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Player player = pair.Value;
            if (player.isAlive)
            {
                nextCell = player.Cell;
                DraggableAcceptor acceptor = nextCell.draggableAcceptor;
                if (acceptor != null)
                {
                    acceptor.AcceptObject(player.gameObject,true); // force accept because target cell's CanAccept() may return false due to pollution
                    player.attachedAcceptor = acceptor;
                }
            }
        }
        StartCoroutine(WaitForPlayersMoveEnd());
    }

    public void UpdatePlayersAndEnemiesState()
    {
        // 敌人伤害玩家早于玩家消灭垃圾
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Player player = pair.Value;
            if (player.isAlive)
            {
                CellController cell = player.Cell;
                Enemy enemy = cell.attachedEnemy;
                if (enemy != null)
                {
                    if (player.type == PlayerType.Collecter)
                    {
                        if (enemy.type == EnemyType.Recyclable)
                        {
                            player.isAlive = false;
                            enemy.isAlive = false;
                            energy++;
                        }
                        else if (enemy.type == EnemyType.Harmless)
                        {
                            player.isAlive = false;
                            enemy.isAlive = false;
                        }
                        else if (enemy.type == EnemyType.Radiation)
                        {
                            player.isAlive = false;
                        }
                    }
                    else if (player.type == PlayerType.Cleaner)
                    {
                        if (enemy.type == EnemyType.Pollution || enemy.type == EnemyType.Radiation)
                        {
                            enemy.isAlive = false;
                        }
                        else if (enemy.type == EnemyType.Recyclable || enemy.type == EnemyType.Harmless)
                        {
                            player.isAlive = false;
                        }
                    }
                }
            }
        }
        // 敌人状态自更新
        foreach (KeyValuePair<int, Enemy> pair in enemies)
        {
            Enemy enemy = pair.Value;
            if (enemy != null && enemy.isAlive)
            {
                enemy.OnStateUpdate();
            }
        }
    }

    public void AddPlayer(Player player)
    {
        players.Add(player.GUID, player);
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy.GUID,enemy);
    }

    public void DeletePlayer(Player player)
    {
        players.Remove(player.GUID);
    }
    public void DeleteEnemy(Enemy enemy)
    {
        enemies.Remove(enemy.GUID);
    }

    IEnumerator WaitForPlayersMoveEnd()
    {
        arePlayersMoving = true;
        yield return new WaitForSeconds(playerMoveTime);
        arePlayersMoving = false;
    }
}
