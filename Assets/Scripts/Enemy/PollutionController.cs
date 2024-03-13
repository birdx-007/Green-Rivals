using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionController : Enemy
{
    private new void Awake()
    {
        base.Awake();
        gameObject.name = "Pollution";
        type = EnemyType.Pollution;
        sourceType = SourceType.None;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
