using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManageSystem : MonoBehaviour
{
    public LevelData currentLevelData;
    public static ManageSystem instance;
    public float playerMoveTime = 0.8f; // 玩家前进一格所需时间
    [NonSerialized]
    public List<float> interactTimes = new(10);
    public UnityEvent OnGameWin;
    public UnityEvent OnGameLose;
    public int leftEnemies = 0;
    public ChessboardController chessboard;
    public SoundSystem soundSystem;

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

    [ReadOnly]
    public bool isProcessing = false; // 回合已开始 各单位正在自主移动交互中
    [NonSerialized] public bool arePlayersMoving = false;

    //private List<Player> players;
    //private List<Enemy> enemies;
    public Dictionary<int, Player> players;
    public Dictionary<int, Enemy> enemies;

    // UI
    public CanvasGroup mainCanvasGroup;

    private void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
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
        if (LevelDatabase.currentLevelData != null)
        {
            energy = LevelDatabase.currentLevelData.energy;
            chessboard.chessboardX = LevelDatabase.currentLevelData.chessboardColNum;
            chessboard.chessboardY = LevelDatabase.currentLevelData.chessboardRowNum;
            chessboard.InitializeChessboard();
            foreach (EnemyInfo enemyInfo in LevelDatabase.currentLevelData.enemies)
            {
                EnemyGenerater.instance.GenerateEnemy(enemyInfo.type, enemyInfo.varition, enemyInfo.row, enemyInfo.col);
            }
            await UniTask.NextFrame();
            chessboard.UpdateChessboard();
        }
        else // 默认
        {
            energy = currentLevelData.energy;
            chessboard.chessboardX = currentLevelData.chessboardColNum;
            chessboard.chessboardY = currentLevelData.chessboardRowNum;
            chessboard.InitializeChessboard();
            foreach (EnemyInfo enemyInfo in currentLevelData.enemies)
            {
                EnemyGenerater.instance.GenerateEnemy(enemyInfo.type, enemyInfo.varition, enemyInfo.row, enemyInfo.col);
            }
            await UniTask.NextFrame();
            chessboard.UpdateChessboard();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isProcessing)
        {
            StartRound();
        }
        mainCanvasGroup.interactable = !isProcessing;
    }

    private bool isWinning()
    {
        if ((leftEnemies == 0||enemies.Count==0))
        {
            return true;
        }

        return false;
    }

    private bool isLosing()
    {
        if ((energy == 0 && enemies.Count > 0 && players.Count == 0 || chessboard.colPolluted[0]))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 开始回合 供玩家摆放完毕后自行执行
    /// </summary>
    public async void StartRound()
    {
        soundSystem.InitiateOnRoundStart();
        isProcessing = true;
        steps++;
        #region MoveAllPlayersForward
        CellController cell = null;
        CellController nextCell = null;
        // players detach former acceptor
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Player player = pair.Value;
            if (player.isAlive)
            {
                cell = player.Cell;
                nextCell = cell.GetNeighbor(CellController.Direction.Right);
                if (nextCell != null)
                {
                    Debug.Log("player move from " + cell.transform.position + " to " + nextCell.transform.position);
                    player.MoveToCell(nextCell, playerMoveTime);
                }
                else
                {
                    Debug.Log("player out of field.");
                    player.isAlive = false;
                    player.OnOutOfField();
                }
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
                    acceptor.AcceptObject(player.gameObject, true); // force accept because target cell's CanAccept() may return false due to pollution
                    player.attachedAcceptor = acceptor;
                }
            }
        }
        await UniTask.Delay(TimeSpan.FromSeconds(playerMoveTime));
        #endregion
        #region UpdatePlayersAndEnemiesState
        // 敌人伤害玩家早于玩家消灭垃圾
        interactTimes.Clear();
        foreach (KeyValuePair<int, Player> pair in players)
        {
            Player player = pair.Value;
            if (player.isAlive)
            {
                cell = player.Cell;
                Enemy enemy = cell.attachedEnemy;
                if (enemy != null)
                {
                    player.Action(enemy).Forget();
                }
            }
        }
        soundSystem.PlayPlayerSound();
        float interactTime = 0f;
        if (interactTimes.Count > 0)
        {
            interactTime = interactTimes.Max() + 0.2f;
        }
        //Debug.Log(interactTime);
        await UniTask.Delay(TimeSpan.FromSeconds(interactTime));
        // 敌人状态自更新
        foreach (KeyValuePair<int, Enemy> pair in enemies)
        {
            Enemy enemy = pair.Value;
            if (enemy != null && enemy.isAlive)
            {
                enemy.OnStateUpdate();
            }
        }
        chessboard.UpdateChessboard();
        await UniTask.Delay(TimeSpan.FromSeconds(0.8f));
        #endregion
        isProcessing = false;
        if (isWinning())
        {
            OnGameWin.Invoke();
        }
        else if (isLosing())
        {
            OnGameLose.Invoke();
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
}
