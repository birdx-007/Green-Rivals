using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationSourceController : Enemy
{
    
    private void Awake()
    {
        gameObject.name = "RadiationSource";
        type = EnemyType.Harmless;
        sourceType = SourceType.RadiationSource;
    }

    public void GenerateRadiation()
    {
        /*
        List<CellController> nineBro = cell.GetNineBro();
        foreach (var broCell in nineBro)
        {
            EnemyGenerater.instance.GenerateGarbage(EnemyType.Radiation,0,broCell.row,broCell.col);
        }
        */
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }
}
