using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum RecordType
{
    None=0,
    Player,
    Enemy
}
public abstract class GameObjectRecordedInManageSystem : MonoBehaviour
{
    public static int nextGUID = 0;
    public int GUID;

    public abstract void AddToManageSystem();

    public abstract void DeleteFromManageSystem();
}
