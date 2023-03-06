using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    protected string tileMapObjName = default;

    protected MapBoard mapController = default;
    protected Tilemap tileMap = default;
    protected List<GameObject> allTileObjs = default;

    public virtual void InitAwake(MapBoard mapController_)
    {
        mapController = mapController_;
        tileMap = gameObject.FindChildComponent<Tilemap>(tileMapObjName);

        allTileObjs = tileMap.gameObject.GetChildrenObjs();
        if (allTileObjs.IsValid())
        {
            allTileObjs.Sort(GFunc.CompareTileObjsToLocalPos2D);
        }
        else { allTileObjs = new List<GameObject>(); }

        /* To do */
    }
}
