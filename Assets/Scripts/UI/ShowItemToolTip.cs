using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
* @Program:ShowItemToolTip.cs
* @Author: Keraz
* @Description:该类用于在鼠标悬停在物品槽上时显示物品工具提示
* @Date: 2025年02月19日 星期三 19:56:24
*/


namespace KFarm.Inventory
{
    // ShowItemToolTip类需要SlotUI组件，并且实现了IPointerEnterHandler和IPointerExitHandler接口
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        // slotUI变量用于存储引用的SlotUI组件
        private SlotUI slotUI;
        // inventoryUI属性用于获取父对象上的InventoryUI组件
        private InventoryUI inventoryUI=>GetComponentInParent<InventoryUI>();

        // Awake方法用于初始化slotUI变量
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        // OnPointerEnter方法在鼠标进入控件区域时调用
        // 该方法检查物品槽中是否有物品，如果有则显示工具提示，否则隐藏工具提示
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDetails!=null)
            {
                // 激活工具提示对象并设置其内容和位置
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetupTooltip(slotUI.itemDetails,slotUI.slotType);
                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
            }
            else
            {
                // 隐藏工具提示对象
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        // OnPointerExit方法在鼠标离开控件区域时调用
        // 该方法隐藏工具提示对象
        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }
}
