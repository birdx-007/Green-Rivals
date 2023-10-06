using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSourceController : Enemy
{
    //public List<PollutionController> pollutions;
    private int pollutionLength;
    private void Awake()
    {
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
        pollutionLength++;
        Cell.SpreadPollution(pollutionLength);
    }
}
