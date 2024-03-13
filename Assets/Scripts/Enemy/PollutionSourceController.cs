using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSourceController : Enemy
{
    //public List<PollutionController> pollutions;
    private int pollutionLength;
    private new void Awake()
    {
        base.Awake();
        gameObject.name = "PollutionSource";
        type = EnemyType.Harmless;
        sourceType = SourceType.PollutionSource;
        //pollutions = new List<PollutionController>();
    }

    private new void Start()
    {
        base.Start();
        pollutionLength = 1;
        Cell.SpreadPollution(pollutionLength);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        GameObject shadow = new GameObject(gameObject.name+" shadow");
        shadow.transform.position = transform.position;
        var shadowSprite = shadow.AddComponent<SpriteRenderer>();
        shadowSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        shadowSprite.DOFade(0, 0.6f);
        shadow.transform.DOScale(3f, 0.6f).OnComplete(() => { Destroy(shadow); });

        pollutionLength++;
        Cell.SpreadPollution(pollutionLength);
    }
}
