using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : GSingleton<PathFinder>
{
    #region 지형 탐색을 위한 변수
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion

    #region A star 알고리즘으로 최단거리를 찾기 위한 변수
    private List<AstarNode> astarResultPath = default;
    private List<AstarNode> astarOpenPath = default;
    private List<AstarNode> astarClosePath = default;
    #endregion

    // 출발지와 목적지 정보로 길을 찾는 함수
    public void FindPath_Astar()
    {
        StartCoroutine(DelayFindPath_Astar(1.0f));
    }
    
    // 탐색 알고리즘에 딜레이를 건다
    private IEnumerator DelayFindPath_Astar(float delay_)
    {
        // A star 알고리즘을 사용하기 위해서 패스 리스트를 초기화한다.
        astarOpenPath = new List<AstarNode>();
        astarClosePath = new List<AstarNode>();
        astarResultPath = new List<AstarNode>();

        TerrainController targetTerrain = default;

        // 출발지의 인덱스를 구해서, 출발지 노드를 찾아온다.
        string[] sourceObjNameParts = sourceObj.name.Split('_');
        int sourceIdx1D = -1;
        int.TryParse(sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIdx1D);
        targetTerrain = mapBoard.GetTerrain(sourceIdx1D);

        // 찾아온 출발지 노드를 Open 리스트에 추가
        AstarNode targetNode = new AstarNode(targetTerrain, destinationObj);
        Add_AstarOpenList(targetNode);

        int loopIdx = 0;
        bool isFoundDestination = false;
        bool isNoWayToGo = false;

        while(isFoundDestination == false && isNoWayToGo == false)
        {
            // Open 리스트를 순회해서 가장 코스트가 낮은 노드를 선택
            AstarNode minCostNode = default;
            foreach(var terrainNode in astarOpenPath)
            {
                if(minCostNode == default)
                {
                    minCostNode = terrainNode;
                }   // 가장 작은 코스트의 노드가 비어있는 경우
                else
                {
                    // terrainNode가 더 작은 코스트를 가지는 경우
                    // minCostNode를 업데이트
                    if (minCostNode.AstarF > terrainNode.AstarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else continue;
                }   // 가장 작은 코스트의 노드가 캐싱되어 있는 경우
            }

            minCostNode.ShowCost_Astar();
            minCostNode.Terrain.SetTileActiveColor(RDefine.TileStatusColor.SEARCH);

            // 선택한 노드가 목적지에 도달했는지 확인
            bool isArriveDest = mapBoard.GetDistance2D(minCostNode.Terrain.gameObject, destinationObj).Equals(Vector2Int.zero);

            if (isArriveDest)
            {
                // 목적지에 도착 했다면 astarResultPath 리스트를 설정
                AstarNode resultNode = minCostNode;
                bool isSet_astarResultPathOk = false;
                while(isSet_astarResultPathOk == false)
                {
                    astarResultPath.Add(resultNode);
                    if(resultNode.AstarPrevNode  == default || resultNode.AstarPrevNode == null)
                    {
                        isSet_astarResultPathOk = true;
                        break;
                    }
                    else
                    {
                        /* Do nothing */
                    }

                    resultNode = resultNode.AstarPrevNode;
                }

                // Open list와 Close list를 정리
                astarOpenPath.Clear();
                astarClosePath.Clear();
                isFoundDestination = true;
                break;
            }
            else
            {
                // 도착하지 않았다면 현재 타일을 기준으로 4 방향 노드를 찾아옴
                List<int> nextSearchIdx1Ds = mapBoard.GetTileIdx2D_Around4Ways(minCostNode.Terrain.TileIdx2D);

                AstarNode nextNode = default;

                foreach(var nextIdx1D in nextSearchIdx1Ds)
                {
                    nextNode = new AstarNode(mapBoard.GetTerrain(nextIdx1D), destinationObj);

                    if (nextNode.Terrain.IsPassable == false) continue;

                    Add_AstarOpenList(nextNode, minCostNode);
                }   // 이동 가능한 노드를 Open list에 추가

                // 탐색이 끝난 노드는 Close list에 추가하고, Open list에서 제거
                // 이 때, Open list가 비어 있다면 더 이상 탐색할 수 없다는 것을 의미

                astarClosePath.Add(minCostNode);
                astarOpenPath.Remove(minCostNode);
                if(astarOpenPath.IsValid() == false)
                {
                    GFunc.LogWarning("[Warning] There are no more tiles to explore.");
                    isNoWayToGo = true;
                }   // 목적지에 도착하지 못했으나, 더 이상 탐색할 수 있는 길이 없는 경우

                foreach(var tempNode in astarOpenPath)
                {
                    GFunc.Log($"Idx: {tempNode.Terrain.TileIdx1D}, "+
                        $"Cost: {tempNode.AstarF}");
                }
            }

            loopIdx++;
            yield return new WaitForSeconds(delay_);
        }   // Astar 알고리즘으로 길을 찾는 메인 루프
    }

    // 비용을 설정한 노드를 Open 리스트에 추가한다
    private void Add_AstarOpenList(AstarNode targetTerrain_, AstarNode prevNode = default)
    {
        // open 리스트에 추가하기 전에 알고리즘 비용을 설정
        Update_AstarCostToTerrain(targetTerrain_, prevNode);

        AstarNode closeNode = astarClosePath.FindNode(targetTerrain_);
        if(closeNode != default && closeNode != null)
        {
            // 이미 탐색이 끝난 좌표의 노드가 존재하는 경우에는
            // Open 리스트에 추가하지 않는다
            /* Do nothing */
        }       // if : close list에 이미 탐색이 끝난 좌표의 노드가 존재하는 경우
        else
        {
            AstarNode openedNode = astarOpenPath.FindNode(targetTerrain_);
            if(openedNode != default && openedNode != null)
            {
                // 타겟 코스트의 코스트가 더 작은 경우에는 Open list에서 노드를 교체한다
                // 타겟 노드의 코스트가 더 큰 경우에는 Open list에 추가하지 않는다
                if(targetTerrain_.AstarF < openedNode.AstarF)
                {
                    astarOpenPath.Remove(openedNode);
                    astarOpenPath.Add(targetTerrain_);
                }
                else
                {
                    /* Do nothing */
                }
            }   // if : Open list에 현재 추가할 노드와 같은 좌표의 노드가 존재하는 경우
            else
            {
                astarOpenPath.Add(targetTerrain_);
            }   // else : Open list에 현재 추가할 노드의 같은 좌표의 노드가 없는 경우
        }
    }

    // Target 지형 정보와 Destination 지형 벙보로 Distance와 Heuristic을 설정하는 함수
    private void Update_AstarCostToTerrain(AstarNode targetNode, AstarNode prevNode)
    {
        // Target 지형에서 Destination 까지의 2D 타일 거리를 계산하는 로직
        Vector2Int distance2D = mapBoard.GetDistance2D(targetNode.Terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        //Heuristic은 직선거리로 고정
        Vector2 localDistance = destinationObj.transform.localPosition - targetNode.Terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);

        // 이전 노드가 존재하는 경우 이전 노드의 코스트를 추가해서 연산한다
        if(prevNode == default || prevNode == null)
        {
            /* Do nothing */
        }
        else
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }

        targetNode.UpdateCost_Astar(totalDistance2D, heuristic, prevNode);
    }
}
