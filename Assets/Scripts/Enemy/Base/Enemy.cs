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
    [NonSerialized] public SourceType sourceType = SourceType.None;
    [NonSerialized] public EnemyType type = EnemyType.None;

    void Awake()
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
        if (!isAlive)
        {
            OnDeath();
            ManageSystem.instance.leftEnemies--;
        }
    }

    protected virtual void OnDeath()
    {
        DeleteFromManageSystem();
        Cell.attachedEnemy = null;
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
