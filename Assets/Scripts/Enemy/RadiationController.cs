using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationController : Enemy
{
    private void Awake()
    {
        gameObject.name = "Radiation";
        type = EnemyType.Radiation;
        sourceType = SourceType.None;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
