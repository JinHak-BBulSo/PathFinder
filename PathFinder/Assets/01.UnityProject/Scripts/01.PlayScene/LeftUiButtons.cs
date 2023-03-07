using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftUiButtons : MonoBehaviour
{
    public void OnClickAstarFindBtn()
    {
        PathFinder.Instance.FindPath_Astar();
    }
}
