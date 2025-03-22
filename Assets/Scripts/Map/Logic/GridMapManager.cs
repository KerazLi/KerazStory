using System;
using System.Collections.Generic;
using MFarm.CropPlant;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("种地的瓦片信息切换")] 
        public RuleTile digTile;
        public RuleTile waterTile;
        private Tilemap digTilemap;
        private Tilemap waterTilemap;
        
        [Header("地图信息")]
        public List<MapData_SO> MapDataList;

        private Season currentSeason;
        
        private Dictionary<string,TileDetails> tileDetailsDict=new();
        private Grid currentGrid;

        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
            EventHandler.GameDayEvent+= OnGameDayEvent;
            EventHandler.RefreshCurrentMap += RefreshMap;
        }
        
        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
            EventHandler.GameDayEvent-= OnGameDayEvent;
            EventHandler.RefreshCurrentMap -= RefreshMap;
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
            foreach (var tile in tileDetailsDict)
            {
                if (tile.Value.daysSinceWatered > -1)
                {
                    tile.Value.daysSinceWatered = -1;
                }
                if (tile.Value.daysSinceDug > -1)
                {
                    tile.Value.daysSinceDug ++;
                }
                //超期消除挖坑
                if (tile.Value.daysSinceDug>5&&tile.Value.seedItemID==-1)
                {
                    tile.Value.daysSinceDug = -1;
                    tile.Value.canDig = true;
                    tile.Value.growthDays = -1;
                }
                if (tile.Value.seedItemID!=-1)
                {
                    tile.Value.growthDays++;
                }
            }
            RefreshMap();
        }


        private void OnAfterSceneLoadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTilemap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();
            //DisplayMap(SceneManager.GetActiveScene().name);
            RefreshMap();
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
                        EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType );
                        break;
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
                        break;
                    case ItemType.Furniture:
                        break;
                    case ItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.daysSinceDug = 0;
                        currentTile.canDig = false;
                        currentTile.canDropItem = false;
                        //TODO：音效
                        break;
                    case ItemType.ChopTool:
                        break;
                    case ItemType.BreakTool:
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daysSinceWatered = 0;
                        break;
                    case ItemType.CollectTool:
                        Crop currentCrop = GetCropObject(mouseWorldPos);
                        currentCrop.ProcessToolAction(itemDetails,currentTile);
                        break;
                    case ItemType.ReapTool:
                        break;
                    case ItemType.ReapableScenery:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            UpdateTileDetails(currentTile);
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

        private Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapPointAll(mouseWorldPos);
            Crop currentCrop = null;
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                if (collider2Ds[i].GetComponent<Crop>())
                {
                    currentCrop = collider2Ds[i].GetComponent<Crop>();
                }
            }
            return currentCrop;
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

        private void SetDigGround(TileDetails tileDetails)
        {
            Vector3Int pos = new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0);
            if (digTilemap!=null)
            {
                digTilemap.SetTile(pos,digTile);
            }
        }
        private void SetWaterGround(TileDetails tileDetails)
        {
            Vector3Int pos = new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0);
            if (waterTilemap!=null)
            {
                waterTilemap.SetTile(pos,waterTile);
            }
        }

        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key=tileDetails.gridX+"X"+tileDetails.gridY+"Y"+SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key]=tileDetails;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void RefreshMap()
        {
            if (digTilemap!=null)
            {
                digTilemap.ClearAllTiles();
            }

            if (waterTilemap!=null)
            {
                waterTilemap.ClearAllTiles();
            }

            foreach (var crop in FindObjectsOfType<Crop>())
            {
                Destroy(crop.gameObject);
            }
            DisplayMap(SceneManager.GetActiveScene().name);
        }

        private void DisplayMap(string sceneName)
        {
            foreach (var tile in tileDetailsDict)
            {
                var key=tile.Key;
                var tileDetaails = tile.Value;
                if (key.Contains(sceneName))
                {
                    if (tileDetaails.daysSinceDug>-1)
                    {
                        SetDigGround(tileDetaails);
                    }
                    if (tileDetaails.daysSinceWatered>-1)
                    {
                        SetWaterGround(tileDetaails);
                    }

                    if (tileDetaails.seedItemID>-1)
                    {
                        EventHandler.CallPlantSeedEvent(tileDetaails.seedItemID,tileDetaails);
                    }
                    //TODO：种子
                }
            }
        }
    }
}
