using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellOccupier
{
    public CellController Cell { get; set; }
}
