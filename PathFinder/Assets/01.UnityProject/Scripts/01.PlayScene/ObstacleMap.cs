using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTileMap";
    private GameObject[] castleObjs = default;
    [SerializeField]
    
    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = OBSTACLE_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);
    }

    private void Start()
    {
        StartCoroutine(DelayStart(0f));
    }

    private IEnumerator DelayStart(float delay)
    {
        yield return null;
        DoStart();
    }

    private void DoStart()
    {
        // 출발지와 목적지를 설정해서 타일을 배치
        castleObjs = new GameObject[2];
        TerrainController[] passableTerrains = new TerrainController[2];

        List<TerrainController> searchTerrains = default;
        int searchIdx = 0;
        TerrainController foundTile = default;

        // 출발지는 좌측에서 우측으로 y 축을 서치해서 빈 지형을 받아옴
        searchIdx = 0;
        foundTile = default;

        while (foundTile == null || foundTile == default)
        {
            //위에서 아래로 서치
            searchTerrains = mapController.GetTerrains_Column(searchIdx, true);

            foreach (TerrainController searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }
            }

            if (foundTile != null || foundTile != default) break;
            if (mapController.MapCellSize.x - 1 <= searchIdx) break;
            searchIdx++;
        }   // 출발지를 찾는 루프
        passableTerrains[0] = foundTile;

        searchIdx = mapController.MapCellSize.x - 1;
        foundTile = default;
        while (foundTile == null || foundTile == default)
        {
            //아래에서 위로 서치

            searchTerrains = mapController.GetTerrains_Column(searchIdx);
            foreach (var searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }
            }

            if (foundTile != null || foundTile != default) break;
            if (searchIdx <= 0) break;
            searchIdx--;
        }
        passableTerrains[1] = foundTile;

        // 출발지와 목적지에 지물을 추가
        GameObject changeTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
        GameObject tempChangeTile = default;
        for (int i = 0; i < 2; i++)
        {
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = string.Format("{0}_{1}",
                changeTilePrefab.name, passableTerrains[i].TileIdx1D);

            tempChangeTile.SetLocalScale(passableTerrains[i].transform.localScale);
            tempChangeTile.SetLocalPos(passableTerrains[i].transform.localPosition);

            // 출발지와 목적지를 캐싱
            castleObjs[i] = tempChangeTile;
            Add_Obstacle(tempChangeTile);
        }   // 출발지와 목적지를 인스턴스화 해서 캐싱하는 루프

        Update_SourDestToPathFinder();
    }

    // 지물을 추가
    public void Add_Obstacle(GameObject obstacle_)
    {
        allTileObjs.Add(obstacle_);
    }

    // 패스파인더에 출발지와 목적지를 설정한다
    public void Update_SourDestToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];
    }
}
