using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionController : Enemy
{
    private void Awake()
    {
        gameObject.name = "Pollution";
        type = EnemyType.Pollution;
        sourceType = SourceType.None;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
