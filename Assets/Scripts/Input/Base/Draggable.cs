using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Draggable : MonoBehaviour
{
    [NonSerialized] public bool isDragging = false;
    private Vector3 offset;
    
    /* 暂时摒弃鼠标拖动方式 改为鼠标点一下spawner则吸附（该状态等价于原拖动） 再点一下等价于鼠标放开
    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        }
        onMouseDownExtra();
    }

    
    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
            onMouseDragExtra();
        }
    }
    
    private void OnMouseUp()
    {
        isDragging = false;
        onMouseUpExtra();
    }
    //*/

    private void OnMouseDown()
    {
        if (!isDragging && Input.GetMouseButton(0))
        {
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;

        }
        else
        {
            isDragging = false;
            onMouseUpExtra();
        }

        onMouseDownExtra();
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
            onMouseDragExtra();
        }
    }

    protected virtual void onMouseDownExtra(){}
    protected virtual void onMouseDragExtra(){}
    protected virtual void onMouseUpExtra(){}

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public DraggableAcceptor GetClosestAcceptor(out float closestDistance)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Map");
        DraggableAcceptor target = null;
        closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsWithTag)
        {
            DraggableAcceptor acceptor = obj.GetComponent<DraggableAcceptor>();

            if (acceptor != null && acceptor.CanAccept())
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = acceptor;
                }
            }
        }
        if (target == null)
        {
            Debug.LogWarning("No closest DraggableAcceptor!");
            closestDistance = Mathf.Infinity;
        }

        return target;
    }
}