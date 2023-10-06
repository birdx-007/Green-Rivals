using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DraggableAcceptor : MonoBehaviour
{
    [FormerlySerializedAs("acceptAll")] public bool canAccept = true;
    public bool hasAcceptedDraggable = false;
    public GameObject acceptedObject = null;

    private void Awake()
    {
        gameObject.tag = "Map";
    }

    public bool CanAccept()
    {
        return canAccept && !hasAcceptedDraggable;
    }

    public void AcceptObject(GameObject obj,bool forceAccept = false)
    {
        if (CanAccept() || forceAccept)
        {
            acceptedObject = obj;
            hasAcceptedDraggable = true;
        }
    }

    public void DetachObject(GameObject obj)
    {
        if (obj.Equals(acceptedObject))
        {
            acceptedObject = null;
            hasAcceptedDraggable = false;
        }
        else
        {
            Debug.Log("You cannot detach this obj from acceptor.");
        }
    }
}
