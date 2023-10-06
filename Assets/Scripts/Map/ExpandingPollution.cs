using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPollution : MonoBehaviour
{

    void Start()
    {
        CellController cellController = GetComponentInParent<CellController>();
        int numberOfCellsToPollute = 3; 
        cellController.SpreadPollution(numberOfCellsToPollute);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
