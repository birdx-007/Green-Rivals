using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerDraggable : PlayerDraggable
{
    protected override void AddPlayerScript(GameObject player)
    {
        base.AddPlayerScript(player);
        player.AddComponent<CleanerController>();
    }
}
