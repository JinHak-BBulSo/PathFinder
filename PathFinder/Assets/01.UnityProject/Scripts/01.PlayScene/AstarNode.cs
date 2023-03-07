using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode
{
    public TerrainController Terrain { get; private set; }
    public GameObject DestinationObj { get; private set; }

    // Astar 알고리즘
    public float AstarF { get; private set; } = float.MaxValue;
    public float AstarG { get; private set; } = float.MaxValue;
    public float AstarH { get; private set; } = float.MaxValue;
    public AstarNode AstarPrevNode { get; private set; } = default;

    public AstarNode(TerrainController terrain_, GameObject destinationObj_)
    {
        Terrain = terrain_;
        DestinationObj = destinationObj_;
    }

    // Astar 알고리즘에 사용할 비용 설정
    public void UpdateCost_Astar(float gCost, float heuristic, AstarNode prevNode)
    {
        float astarF = gCost + heuristic;

        if(astarF < AstarF)
        {
            AstarG = gCost;
            AstarH = heuristic;
            AstarF = astarF;
            AstarPrevNode = prevNode;
        }
        else
        {
            /* Do nothing */
        }
    }

    public void ShowCost_Astar()
    {
        GFunc.Log($"TileIdx1D: {Terrain.TileIdx1D}, " + $"F : {AstarF}, G : {AstarG}, H : {AstarH}");
    }
}
