using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

[RequireComponent(typeof(DraggableAcceptor))]
public class CellController : MonoBehaviour
{
    public float edgeX = 1.5f;
    public float edgeY = 1f;
    public int row; //行
    public int col; //列
    private bool isUsingGreenSprite = false;
    public DraggableAcceptor draggableAcceptor;
    public Enemy attachedEnemy;
    public Player attachedPlayer;
    private static int chessboardX;
    private static int chessboardY;
    //public Sprite pollutedSprite;
    public List<Sprite> greenSprites;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer pollution;

    private void Awake()
    {
        draggableAcceptor = GetComponent<DraggableAcceptor>();
        chessboardX = GetComponentInParent<ChessboardController>().chessboardX;
        chessboardY = GetComponentInParent<ChessboardController>().chessboardY;
        //Debug.Log("SIZE: " + chessboardSize);
        spriteRenderer = GetComponent<SpriteRenderer>();
        //SetState(col >= ChessboardController.instance.pollutedStartCol);
        draggableAcceptor.canAccept = isUsingGreenSprite;
        attachedEnemy = null;
        attachedPlayer = null;
    }

    private void Update()
    {
        draggableAcceptor.canAccept = isUsingGreenSprite;
    }

    public void SetState(bool isPolluted)
    {
        if (!isUsingGreenSprite && !isPolluted)
        {
            spriteRenderer.sprite = GetGreenSprite();
            pollution.DOFade(0, 0.4f);
        }
        else if (isPolluted)
        {
            //spriteRenderer.sprite = pollutedSprite;
            pollution.DOFade(1, 0.4f);
        }
        isUsingGreenSprite = !isPolluted;
    }

    private Sprite GetGreenSprite()
    {
        Random random = new Random();
        return greenSprites[random.Next(0, greenSprites.Count)];
    }

    public void SpreadPollution(int distance)
    {
        //Debug.Log("HERE");
        CellController topCell = this;
        CellController bottomCell = this;
        CellController leftCell = this;
        CellController rightCell = this;

        for (int i = 0; i < distance; i++)
        {
            // Get Neighbor
            if (topCell != null)
            {
                topCell = topCell.GetNeighbor(Direction.Up);
            }

            if (bottomCell != null)
            {
                bottomCell = bottomCell.GetNeighbor(Direction.Down);
            }

            if (leftCell != null)
            {
                leftCell = leftCell.GetNeighbor(Direction.Left);
            }

            if (rightCell != null)
            {
                rightCell = rightCell.GetNeighbor(Direction.Right);
            }
            // pollute neighbor
            if (topCell != null)
            {
                EnemyGenerater.instance.GenerateEnemy(EnemyType.Pollution,0,topCell.row,topCell.col);
            }

            if (bottomCell != null)
            {
                EnemyGenerater.instance.GenerateEnemy(EnemyType.Pollution,0,bottomCell.row,bottomCell.col);
            }

            if (leftCell != null)
            {
                EnemyGenerater.instance.GenerateEnemy(EnemyType.Pollution, 0, leftCell.row,leftCell.col);
            }

            if (rightCell != null)
            {
                EnemyGenerater.instance.GenerateEnemy(EnemyType.Pollution,0,rightCell.row,rightCell.col);
            }
        }
    }

    public CellController GetNeighbor(Direction direction)
    {
        int row = this.row;
        int col = this.col;

        switch (direction)
        {
            case Direction.Up:
                return ChessboardController.instance.GetCell(row - 1, col);
            case Direction.Down:
                return ChessboardController.instance.GetCell(row + 1, col);
            case Direction.Left:
                return ChessboardController.instance.GetCell(row, col - 1);
            case Direction.Right:
                return ChessboardController.instance.GetCell(row, col + 1);
        }
        return null;
    }

    public List<CellController> GetNineBro()
    {
        List<CellController> res = new List<CellController>();
        CellController cell1 = ChessboardController.instance.GetCell(row-1,col-1);
        if (cell1!=null)
        {
            res.Add(cell1);
        }
        CellController cell2 = ChessboardController.instance.GetCell(row-1,col);
        if (cell2!=null)
        {
            res.Add(cell2);
        }
        CellController cell3 = ChessboardController.instance.GetCell(row-1,col+1);
        if (cell3!=null)
        {
            res.Add(cell3);
        }
        CellController cell4 = ChessboardController.instance.GetCell(row,col-1);
        if (cell4!=null)
        {
            res.Add(cell4);
        }
        CellController cell5 = ChessboardController.instance.GetCell(row,col+1);
        if (cell5!=null)
        {
            res.Add(cell5);
        }
        CellController cell6 = ChessboardController.instance.GetCell(row+1,col-1);
        if (cell6!=null)
        {
            res.Add(cell6);
        }
        CellController cell7 = ChessboardController.instance.GetCell(row+1,col);
        if (cell7!=null)
        {
            res.Add(cell7);
        }
        CellController cell8 = ChessboardController.instance.GetCell(row+1,col+1);
        if (cell8!=null)
        {
            res.Add(cell8);
        }

        return res;
    }

    /* // 似乎出了问题 暂时弃用 已在chessboard中实现相同功能函数
    private CellController GetCell(int targetRow, int targetCol)
    {
        Debug.Log("targetCol" + targetCol + "targetRow" + targetRow);
        GameObject cellObject = GameObject.Find("Cell_" + targetRow + "_" + targetCol);
        if (cellObject != null)
        {
            return cellObject.GetComponent<CellController>();
        }
        return null;
    }
    //*/

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
