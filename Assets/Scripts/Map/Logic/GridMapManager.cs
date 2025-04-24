using System;
using System.Collections.Generic;
using MFarm.CropPlant;
using MFrarm.CropPlant;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Utilities;

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
        //场景第一次加载
        private Dictionary<string, bool> firstLoadDict = new();
        private Grid currentGrid;
        private List<ReapItem> itemsInRadius;

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
        private void Start()
        {
            foreach (var mapData in MapDataList)
            {
                firstLoadDict.Add(mapData.sceneName,true);
                InitTileDetialsDict(mapData);
            }
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
            if (firstLoadDict[SceneManager.GetActiveScene().name])
            {
                EventHandler.CallGenerateCropEvent();
                firstLoadDict[SceneManager.GetActiveScene().name] = false;
            }
            //EventHandler.CallGenerateCropEvent();
            RefreshMap();
        }

        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);
            if (currentTile!=null)
            {
                Crop currentCrop = GetCropObject(mouseWorldPos);
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
                    case ItemType.BreakTool:
                    case ItemType.ChopTool:
                        currentCrop?.ProcessToolAction(itemDetails,currentCrop.tileDetails);
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daysSinceWatered = 0;
                        break;
                    case ItemType.CollectTool:
                        
                        currentCrop?.ProcessToolAction(itemDetails,currentTile);
                        break;
                    case ItemType.ReapTool:
                        var reapCount = 0;
                        for (int i = 0; i < itemsInRadius.Count; i++)
                        {
                            EventHandler.CallParticleEffectEvent(ParticaleEffectType.ReapableScenery,itemsInRadius[i].transform.position+Vector3.up);
                            itemsInRadius[i].SpawnHarvestItems();
                            Destroy(itemsInRadius[i].gameObject);
                            reapCount++;
                            if (reapCount>=Setting.reapAmount)
                            {
                                break;
                            }
                        }
                        break;
                    case ItemType.ReapableScenery:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            UpdateTileDetails(currentTile);
        }

        /// <summary>
        /// 返回工具范围内的杂草
        /// </summary>
        /// <param name="tool">物品</param>
        /// <returns></returns>
        public bool HaveReapableItemsInRadius(Vector3 mouseWorldPos,ItemDetails tool)
        {
            itemsInRadius = new List<ReapItem>();
            Collider2D[] collider2Ds = new Collider2D[20];
            Physics2D.OverlapCircleNonAlloc(mouseWorldPos, tool.itemUseRadius, collider2Ds);
            if (collider2Ds.Length>0)
            {
                for (int i = 0; i < collider2Ds.Length; i++)
                {
                    if (collider2Ds[i] != null)
                    {
                        if (collider2Ds[i].GetComponent<ReapItem>())
                        {
                            var item = collider2Ds[i].GetComponent<ReapItem>();
                            itemsInRadius.Add(item);
                        }
                    }
                }
            }
            return itemsInRadius.Count>0;
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

        public Crop GetCropObject(Vector3 mouseWorldPos)
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
        public TileDetails GetTileDetails(string key)
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

        public void UpdateTileDetails(TileDetails tileDetails)
        {
            string key=tileDetails.gridX+"X"+tileDetails.gridY+"Y"+SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key]=tileDetails;
            }
            else
            {
                tileDetailsDict.Add(key,tileDetails);
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
        /// <summary>
        /// 根据场景名字构建网格范围，输出范围和远点
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <param name="gridDimensions">网格范围</param>
        /// <param name="gridOrigin">网格原点</param>
        /// <returns>是否有当前场景信息</returns>
        public bool GetGridDimensions(string sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin)
        {
            gridDimensions = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;
            foreach (var mapData in MapDataList)
            {
                if (mapData.sceneName==sceneName)
                {
                    gridDimensions.x=mapData.gridWidth;
                    gridDimensions.y=mapData.gridHeight;
                    gridOrigin.x=mapData.originX;
                    gridOrigin.y=mapData.originY;
                    return true;
                }
            }
            return false;
        }
    }
}
