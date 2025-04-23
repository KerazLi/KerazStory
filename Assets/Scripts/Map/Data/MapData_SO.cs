using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="MapData_SO",menuName = "Map/MapData")]
public class MapData_SO : ScriptableObject
{
    [SceneName] public string sceneName;
    [Header("地图信息")]
    public int gridWidth;
    public int gridHeight;
    [Header("左下角的原点")]
    public int originX;
    public int originY;
    [Header("地图块信息")]
    public List<TileProperty> tileProperties;
}
