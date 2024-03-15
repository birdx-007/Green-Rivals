using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboardController : MonoBehaviour
{
    public static ChessboardController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
        cells = new List<List<CellController>>();
        CellController cell = cellPrefab.GetComponent<CellController>();
        if (cell!= null)
        {
            cellX = cell.edgeX;
            cellY = cell.edgeY;
        }
    }
    public int chessboardX = 8;
    public int chessboardY = 5;
    //public int pollutedStartCol = 4;
    public GameObject cellPrefab;
    private List<List<CellController>> cells;
    private float cellX;
    private float cellY;
    [ReadOnly] public List<bool> colPolluted;

    public void InitializeChessboard()
    {
        cells = new List<List<CellController>>();
        for (int i = 0; i < chessboardY; i++)
        {
            cells.Add(new List<CellController>());
            for (int j = 0; j < chessboardX; j++)
            {
                if (chessboardY == 5)
                {
                    if (i == 0 && j == 0 || i == chessboardY - 1 && j == 0)
                    {
                        cells[i].Add(null);
                        continue;
                    }
                }
                
                GameObject obj = Instantiate(cellPrefab, transform);
                CellController cell = obj.GetComponent<CellController>();
                cell.row = i + 1;
                cell.col = j + 1;
                cells[i].Add(cell);
                //obj.transform.localPosition = new Vector3((0.5f + j) * cell.edgeX, -(0.5f + i) * cell.edgeY, 0);
                obj.transform.localPosition = new Vector3((0.5f + j - chessboardX*0.5f) * cell.edgeX, -(0.5f + i - chessboardY * 0.5f) * cell.edgeY, 0);
                obj.name = "Cell_" + cell.row + "_" + cell.col;
                SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
                renderer.sortingLayerName = "Map";
                //renderer.sortingOrder = -i; // 调节project settings的graphics的排序轴为010就不用这句了
            }
        }
        colPolluted = new List<bool>(chessboardX);
        for (int i = 1; i <= chessboardX; i++)
        {
            colPolluted.Add(false);
        }
    }

    public CellController GetCell(int targetRow, int targetCol)
    {
        if (targetRow >= 1 && targetRow <= chessboardY && targetCol >= 1 && targetCol <= chessboardX)
        {
            return cells[targetRow - 1][targetCol - 1];
        }
        return null;
    }

    public CellController GetCellAtPosition(Vector3 position)
    {
        Vector3 chessboardLocalPosition = position - transform.position;
        int row = Mathf.RoundToInt((-chessboardLocalPosition.y+0.5f) / cellY);
        int col = Mathf.RoundToInt((chessboardLocalPosition.x+0.5f) / cellX);
        return GetCell(row, col);
    }

    public Vector3 GetCellPosition(int targetRow, int targetCol)
    {
        CellController cell = GetCell(targetRow, targetCol);
        if (cell != null)
        {
            return cell.transform.position;
        }
        Debug.LogError("fail to get cell pos!");
        return new Vector3();
    }

    public void UpdateChessboard()
    {
        for (int i = 1; i <= chessboardX; i++)
        {
            bool isColPolluted = false;
            for (int j = 1; j <= chessboardY; j++)
            {
                CellController cell = GetCell(j, i);
                if (cell != null && cell.attachedEnemy != null)
                {
                    if (isColPolluted == false)
                    {
                        isColPolluted = true;
                    }
                }
            }
            colPolluted[i - 1] = isColPolluted;
        }

        for (int i = 1; i <= chessboardX; i++)
        {
            for (int j = 1; j <= chessboardY; j++)
            {
                CellController cell = GetCell(j, i);
                if (cell != null)
                {
                    cell.SetState(colPolluted[i - 1]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        //Gizmos.DrawWireCube(transform.position + new Vector3(0.5f * chessboardX * 1.8f, -0.5f * chessboardY * 1.5f, 0),
            //new Vector3(chessboardX * 1.8f, chessboardY * 1.5f, 0));
        Gizmos.DrawWireCube(transform.position, new Vector3(chessboardX * 1.8f, chessboardY * 1.5f, 0));
    }
}
