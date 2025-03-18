using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("地图信息")]
        public List<MapData_SO> MapDataList;
        private Dictionary<string,TileDetails> tileDetailsDict=new();
        private Grid currentGrid;

        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }

        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
        }

        private void OnAfterSceneLoadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);
            if (currentTile!=null)
            {
                //WORKFLOW:物品使用实际功能
                switch (itemDetails.itemType)
                {
                    case ItemType.Seed:
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos);
                        break;
                    case ItemType.Furniture:
                        break;
                    case ItemType.HoeTool:
                        break;
                    case ItemType.ChopTool:
                        break;
                    case ItemType.BreakTool:
                        break;
                    case ItemType.WaterTool:
                        break;
                    case ItemType.CollectTool:
                        break;
                    case ItemType.ReapTool:
                        break;
                    case ItemType.ReapableScenery:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Start()
        {
            foreach (var mapData in MapDataList)
            {
                InitTileDetialsDict(mapData);
            }
        }

        private void InitTileDetialsDict(MapData_SO mapDataSo)
        {
            foreach (TileProperty tileProperty in mapDataSo.tileProperties)
            {
                TileDetails tileDetails=new TileDetails
                {
                    gridX=tileProperty.tileCoordinate.x,
                    gridY=tileProperty.tileCoordinate.y
                }; 
                //字典的Key
                string key=tileProperty.tileCoordinate.x+"X"+tileProperty.tileCoordinate.y+"Y"+mapDataSo.sceneName;
                if (GetTileDetails(key)!=null)
                {
                    tileDetails = GetTileDetails(key);
                }
                switch (tileProperty.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key)!=null)
                {
                    tileDetailsDict[key]=tileDetails;
                }
                else
                {
                    tileDetailsDict.Add(key,tileDetails);
                }
            }
            
        }

        /// <summary>
        /// 获取指定键的瓷砖详细信息。
        /// </summary>
        /// <param name="key">瓷砖详细信息的唯一键。</param>
        /// <returns>如果找到，则返回对应的TileDetails对象；否则返回null。</returns>
        private TileDetails GetTileDetails(string key)
        {
            // 检查字典中是否包含给定键的瓷砖详细信息
            if (tileDetailsDict.ContainsKey(key))
            {
                // 如果找到，则返回对应的瓷砖详细信息
                return tileDetailsDict[key];
            }
        
            // 如果未找到，则返回null
            return null;
        }
        /// <summary>
        /// 根据鼠标在网格上的位置获取瓷砖详细信息。
        /// </summary>
        /// <param name="mouseGridPos">鼠标在网格上的位置，使用三维整数表示。</param>
        /// <returns>返回对应位置的瓷砖详细信息。</returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            // 构造一个唯一的键，包含鼠标位置的x和y坐标以及当前场景的名称。
            string key=mouseGridPos.x+"X"+mouseGridPos.y+"Y"+SceneManager.GetActiveScene().name;
            
            // 调用GetTileDetails方法，使用构造的键来获取瓷砖详细信息并返回。
            return GetTileDetails(key);
        }
    }
}
