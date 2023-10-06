using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum PlayerType
{
    None = 0,
    Collecter,
    Cleaner
}

[RequireComponent(typeof(BoxCollider2D))]
public class Player : GameObjectRecordedInManageSystem, ICellOccupier
{
    public CellController Cell { get; set; }
    [NonSerialized] public bool isAlive = true;
    public float moveTime = 0.5f;
    public PlayerType type = PlayerType.None;
    protected Animator animator;
    public DraggableAcceptor attachedAcceptor = null;

    protected void Start()
    {
        AddToManageSystem();
    }
    private void Update()
    {
        if (!isAlive)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        DeleteFromManageSystem();
        Cell.attachedPlayer = null;
        if (attachedAcceptor != null)
        {
            attachedAcceptor.DetachObject(gameObject);
            attachedAcceptor = null;
        }
        Destroy(gameObject);
    }

    public void MoveToCell(CellController targetCell,float time)
    {
        Cell.attachedPlayer = null;
        Cell = targetCell;
        Cell.attachedPlayer = this;
        if (attachedAcceptor != null)
        {
            attachedAcceptor.DetachObject(gameObject);
            attachedAcceptor = null;
        }
        MoveTo(targetCell.transform.position,time);
    }
    public void MoveTo(Vector3 target,float time)
    {
        transform.DOMove(target, time);
    }

    public override void AddToManageSystem()
    {
        GUID = nextGUID;
        nextGUID++;
        ManageSystem.instance.AddPlayer(this);
    }

    public override void DeleteFromManageSystem()
    {
        ManageSystem.instance.DeletePlayer(this);
    }
}
