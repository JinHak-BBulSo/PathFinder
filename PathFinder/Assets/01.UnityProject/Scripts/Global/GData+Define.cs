using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GData
{
    public const string SCENE_NAME_TITLE = "00.TitleScene";
    public const string SCENE_NAME_PLAY = "01.PlayScene";
}

// 지형의 속성을 정의
public enum TerrainType
{
    NONE = -1,
    PLAIN_PASS,
    OCEAN_N_PASS
}