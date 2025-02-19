using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
* @Program:GridMap.cs
* @Author: Keraz
* @Description:在编辑模式下执行，用于处理网格地图的数据
* @Date: 2025年02月19日 星期三 22:46:45
*/

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    // 地图数据资产
    public MapData_SO mapData;
    // 网格类型
    public GridType gridType;
    // 当前的瓦片地图组件
    private Tilemap currentTilemap;

    // 在组件启用时调用
    private void OnEnable()
    {
        // 如果当前不是在播放模式下
        if (!Application.IsPlaying(this))
        {
            // 获取组件上的瓦片地图
            currentTilemap = GetComponent<Tilemap>();

            // 如果地图数据已指定，则清空其瓦片属性
            if (mapData != null)
                mapData.tileProperties.Clear();
        }
    }

    // 在组件禁用时调用
    private void OnDisable()
    {
        // 如果当前不是在播放模式下
        if (!Application.IsPlaying(this))
        {
            // 获取组件上的瓦片地图
            currentTilemap = GetComponent<Tilemap>();

            // 更新瓦片属性
            UpdateTileProperties();
#if UNITY_EDITOR
            // 如果地图数据已指定，则标记为已更改
            if (mapData != null)
                EditorUtility.SetDirty(mapData);
#endif
        }
    }

    // 更新瓦片属性的方法
    private void UpdateTileProperties()
    {
        // 压缩瓦片地图的边界
        currentTilemap.CompressBounds();

        // 如果当前不是在播放模式下
        if (!Application.IsPlaying(this))
        {
            // 检查地图数据是否已指定
            if (mapData != null)
            {
                // 已绘制范围的左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                // 已绘制范围的右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;

                // 遍历瓦片地图上的每个瓦片
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        // 获取当前瓦片
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

                        // 如果当前瓦片存在
                        if (tile != null)
                        {
                            // 创建新的瓦片属性并添加到地图数据中
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true
                            };

                            mapData.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
