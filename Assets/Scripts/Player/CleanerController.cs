using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : Player
{
    private void Awake()
    {
        gameObject.name = "Cleaner";
        type = PlayerType.Cleaner;
        animator = GetComponent<Animator>();
    }
}
