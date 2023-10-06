using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDraggable : Draggable
{
    //public Cleaner 
    public float maxDisplayShadowDistance = 1.2f;
    private GameObject shadowObject;
    private DraggableAcceptor targetDraggableAcceptor;

    private void Awake()
    {
        // initialize renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Player";
        // initialize shadow
        shadowObject = new GameObject("Shadow");
        SpriteRenderer shadowSpriteRenderer = shadowObject.AddComponent<SpriteRenderer>();
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        Color color = spriteRenderer.color;
        shadowSpriteRenderer.color = new Color(color.r, color.g, color.b, color.a * 0.5f);
        shadowSpriteRenderer.sortingLayerName = "Player";
        shadowObject.SetActive(false);
        targetDraggableAcceptor = null;
    }

    protected override void onMouseDragExtra()
    {
        base.onMouseDragExtra();
        float closestDistance;
        targetDraggableAcceptor = GetClosestAcceptor(out closestDistance);
        if (targetDraggableAcceptor != null && targetDraggableAcceptor.CanAccept() && closestDistance <= maxDisplayShadowDistance)
        {
            shadowObject.transform.position = targetDraggableAcceptor.transform.position;
            shadowObject.SetActive(true);
        }
        else
        {
            shadowObject.SetActive(false);
            targetDraggableAcceptor = null;
        }
    }

    protected override void onMouseUpExtra()
    {
        base.onMouseDragExtra();
        if (Input.GetMouseButton(0))
        {
            if (targetDraggableAcceptor != null && targetDraggableAcceptor.CanAccept())
            {
                GameObject playerObject = new GameObject();
                playerObject.transform.position = targetDraggableAcceptor.transform.position;
                // add SpriteRenderer
                SpriteRenderer spriteRenderer = playerObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
                Color color = GetComponent<SpriteRenderer>().color;
                spriteRenderer.color = color;
                spriteRenderer.sortingLayerName = "Player";
                // add Animator
                Animator animator = playerObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
                // add Script
                AddPlayerScript(playerObject);
                Player player = playerObject.GetComponent<Player>();
                targetDraggableAcceptor.AcceptObject(playerObject);
                player.attachedAcceptor = targetDraggableAcceptor;
                player.Cell = targetDraggableAcceptor.GetComponent<CellController>();
                player.Cell.attachedPlayer = player;
                ManageSystem.instance.energy--;
            }
        }
        Destroy(shadowObject);
        Destroy(gameObject);
    }

    protected virtual void AddPlayerScript(GameObject player)
    {
        //player.AddComponent<Player>();
    }
}
