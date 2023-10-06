using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollecterDraggable : PlayerDraggable
{
    protected override void AddPlayerScript(GameObject player)
    {
        base.AddPlayerScript(player);
        player.AddComponent<CollecterController>();
    }
}
