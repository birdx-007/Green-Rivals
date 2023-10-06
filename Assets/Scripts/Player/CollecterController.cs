using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollecterController : Player
{
    private void Awake()
    {
        gameObject.name = "Collecter";
        type = PlayerType.Collecter;
        animator = GetComponent<Animator>();
    }
}
