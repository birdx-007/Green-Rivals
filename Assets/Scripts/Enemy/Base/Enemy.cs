using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None = 0,
    Recyclable,
    Harmless,
    Radiation,
    Pollution
}

public enum SourceType
{
    None = 0,
    PollutionSource,
    RadiationSource
}

//[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : GameObjectRecordedInManageSystem,ICellOccupier
{
    public CellController Cell { get; set; }
    [NonSerialized] public bool isAlive = true;
    private bool isDying = false;
    [NonSerialized] public SourceType sourceType = SourceType.None;
    [NonSerialized] public EnemyType type = EnemyType.None;

    protected void Awake()
    {
        // 登场小动画
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.8f).SetEase(Ease.OutBack);
    }

    protected void Start()
    {
        AddToManageSystem();
    }

    private void Update()
    {
        if (!isAlive && !isDying)
        {
            isDying = true;
            OnDeath();
        }
    }

    protected async void OnDeath()
    {
        DeleteFromManageSystem();
        Cell.attachedEnemy = null;
        await transform.DOScale(0, 0.8f).AsyncWaitForCompletion();
        ManageSystem.instance.leftEnemies--;
        Destroy(gameObject);
    }

    public virtual void OnStateUpdate()
    {
        
    }
    public override void AddToManageSystem()
    {
        GUID = nextGUID;
        nextGUID++;
        ManageSystem.instance.AddEnemy(this);
    }

    public override void DeleteFromManageSystem()
    {
        ManageSystem.instance.DeleteEnemy(this);
    }
}
