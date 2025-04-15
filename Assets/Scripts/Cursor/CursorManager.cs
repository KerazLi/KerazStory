using System;
using System.Collections;
using System.Collections.Generic;
using MFarm.CropPlant;
using MFarm.Map;
using MFram.CropPlant;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed,item;
    private Sprite currentSprite;
    private Image cursorImage;
    private RectTransform cursorCanvas;
    private Camera mainCamera;
    private Grid currentGrid;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos=new(0,0,0);
    private bool cursorEnable;
    private bool cursorPositionValid;
    private ItemDetails currentItem;
    private Transform PlayerTransform => FindObjectOfType<PlayerController>().transform;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneloadEvent;
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.Find("CursorImage").GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
    }

    private void Update()
    {
        if (cursorCanvas==null)
        {
            Debug.LogWarning("Cursor canvas is null");
            return;
        }
        cursorImage.transform.position = Input.mousePosition;
        // 检查当前是否有UI元素被用户交互
        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }
        // 检查当前是否有UI元素被用户交互、
        //SetCursorImage(InteractWithUI()?normal:currentSprite);
    }

    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0)&&cursorPositionValid)
        {
            //执行方法
            EventHandler.CallMouseClickedEvent(mouseWorldPos,currentItem);
        }
    }

    #region 鼠标样式
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new(1,1,1,1);
    }
    /// <summary>
    /// 鼠标可用
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new(1,1,1,1);
    }
    /// <summary>
    /// 鼠标不可用
    /// </summary>
    private void SetCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new(1,0,0,0.5f);
    }
    

    #endregion
    
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        
        if (!isSelected)
        {
            currentSprite = normal;
            currentItem = null;
            cursorEnable = false;
        }
        else //物品被选中才切换图片
        {
            currentItem = itemDetails;
            //currentItem = itemDetails;
            //WORKFLOW:添加所有类型对应的鼠标图片
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.Furniture => tool,
                ItemType.CollectTool => tool,
                _ => normal,
            };
            cursorEnable = true;
        }
    }

    /// <summary>
    /// 检查当前是否有UI元素被用户交互
    /// </summary>
    /// <returns>返回一个布尔值，指示是否有UI元素正在被交互</returns>
    private bool InteractWithUI()
    {
        // 检查当前是否存在一个事件系统，并且鼠标指针是否位于游戏对象上
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // 如果条件满足，返回true，表示当前有UI元素被交互
            return true;
        }
        else
        {
            // 如果条件不满足，返回false，表示当前没有UI元素被交互
            return false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckCursorValid()
    {
        TileDetails tileDetails = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        var playerGridPos=currentGrid.WorldToCell(PlayerTransform.position);
        if (Mathf.Abs(playerGridPos.x-mouseGridPos.x)>currentItem.itemUseRadius||Mathf.Abs(playerGridPos.y-mouseGridPos.y)>currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }
        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        if (currentTile!=null)
        {
            CropDetails currentCrop=CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);
            switch (currentItem.itemType)
            {
                case ItemType.Seed:
                    if (currentTile.daysSinceDug>-1&&currentTile.seedItemID==-1)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.Commodity:
                    if (currentTile.canDropItem&&currentItem.canDropped)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.Furniture:
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daysSinceDug>-1&&currentTile.daysSinceWatered==-1)
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.BreakTool:
                case ItemType.ChopTool:
                    if (crop!=null)
                    {
                        if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currentItem.itemID))
                        {
                            SetCursorValid();
                        }else
                        {
                            SetCursorInvalid();
                        }
                    }
                    break;
                case ItemType.CollectTool:
                    if (currentCrop!=null)
                    {
                        if (currentCrop.CheckToolAvailable(currentItem.itemID))
                        {
                            if (currentTile.growthDays>=currentCrop.TotalGrowthDays)
                            {
                                SetCursorValid();
                            }
                            else
                            {
                                SetCursorInvalid();
                            }
                        }
                        
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.ReapTool:
                    if (GridMapManager.Instance.HaveReapableItemsInRadius(currentItem))
                    {
                        SetCursorValid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
                case ItemType.ReapableScenery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            SetCursorInvalid();
            //Debug.Log("111");
        }
    }

    private void OnAfterSceneloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }
}
