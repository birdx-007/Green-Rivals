using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationSourceController : Enemy
{
    
    private new void Awake()
    {
        base.Awake();
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
        GameObject shadow = new GameObject(gameObject.name + " shadow");
        shadow.transform.position = transform.position;
        var shadowSprite = shadow.AddComponent<SpriteRenderer>();
        shadowSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        shadowSprite.DOFade(0, 0.6f);
        shadow.transform.DOScale(3f, 0.6f).OnComplete(() => { Destroy(shadow); });
    }
}
