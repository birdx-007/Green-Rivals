using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public enum PlayerType
{
    None = 0,
    Collecter,
    Cleaner
}

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Player : GameObjectRecordedInManageSystem, ICellOccupier
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

    private void OnDestroy()
    {
        DeleteFromManageSystem();
    }

    protected void DetachCell()
    {
        Cell.attachedPlayer = null;
        if (attachedAcceptor != null)
        {
            attachedAcceptor.DetachObject(gameObject);
            attachedAcceptor = null;
        }
    }

    public abstract UniTask Action(Enemy enemy);

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
    public async void MoveTo(Vector3 target,float time)
    {
        animator.SetBool("isWalking", true);
        await transform.DOMove(target, time).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        animator.SetBool("isWalking", false);
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

    public async void OnOutOfField()
    {
        GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.4f).SetEase(Ease.InQuad);
        await transform.DOMoveX(transform.position.x + 1.5f, 0.4f).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
