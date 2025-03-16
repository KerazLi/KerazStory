using System;
using System.Collections;
using System.Collections.Generic;
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
    private Vector3Int mouseGridPos;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneUnloadEvent;
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
        if (!InteractWithUI())
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
        }
        else
        {
            SetCursorImage(normal);
        }
        // 检查当前是否有UI元素被用户交互、
        //SetCursorImage(InteractWithUI()?normal:currentSprite);
    }

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new(1,1,1,1);
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentSprite = normal;
        }
        else //物品被选中才切换图片
        {
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

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        Debug.Log("WorldPos: "+mouseWorldPos+" GridPos"+mouseGridPos);
    }

    private void OnAfterSceneUnloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }
}
