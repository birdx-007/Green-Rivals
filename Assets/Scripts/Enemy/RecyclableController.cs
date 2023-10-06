using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclableController : Enemy
{
    private void Awake()
    {
        gameObject.name = "Recyclable";
        type = EnemyType.Recyclable;
        sourceType = SourceType.None;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
