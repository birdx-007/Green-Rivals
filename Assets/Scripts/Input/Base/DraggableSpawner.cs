using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSpawner : MonoBehaviour, IPointerDownHandler
{
    // Must be added to UI
    public GameObject spawningDraggableObject; // 相应的PlayerDraggable预制体
    public bool canSpawn = true;
    private bool hasSpawnedOne = false;
    private Draggable DraggableHasSpawned;

    private void Update()
    {
        canSpawn = ManageSystem.instance.energy > 0;
        if (hasSpawnedOne)
        {
            if (!DraggableHasSpawned.isDragging)
            {
                hasSpawnedOne = false;
                DraggableHasSpawned = null;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canSpawn && !hasSpawnedOne && spawningDraggableObject is not null && Input.GetMouseButton(0))
        {
            hasSpawnedOne = true;
            GameObject obj = Instantiate(spawningDraggableObject, GetMouseWorldPosition(), Quaternion.identity);
            DraggableHasSpawned = obj.GetComponent<Draggable>();
            DraggableHasSpawned.isDragging = true;
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
