using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* @Program:SlotUI.cs
* @Author: Keraz
* @Description:SlotUI类负责单个库存槽的UI逻辑，实现了点击和拖拽事件接口
* @Date: 2025年02月19日 星期三 19:58:29
*/

namespace KFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        // 组件引用
        [Header("组件获取")] 
        [SerializeField] private Image slotImage; // 槽的图像组件
        [SerializeField] private TextMeshProUGUI amountText; // 显示物品数量的文本组件
        public Image slotHightLight; // 槽的高亮图像组件
        [SerializeField] private Button button; // 槽的按钮组件
        
        // 格子类型
        [Header("格子类型")] 
        public SlotType slotType; // 槽的类型（例如：背包、装备等）
        
        // 是否选中
        public bool isSelected; // 当前槽是否被选中

        // 物品信息
        public ItemDetails itemDetails; // 槽中物品的详细信息
        public int itemAmount; // 槽中物品的数量
        public int slotIndex; // 槽在库存中的索引
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>(); // 获取父对象的InventoryUI组件

        // 初始化槽UI
        private void Start()
        {
            isSelected = false;
            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                inventoryUI.UpdateSlotHightlight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        /// <summary>
        /// 处理指针点击事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0)
            {
                return;
            }
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHightlight(slotIndex);
            if (slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        /// <summary>
        /// 处理开始拖拽事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true;
                inventoryUI.dragItem.sprite = slotImage.sprite;
                inventoryUI.dragItem.SetNativeSize();
                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);
            }
        }

        /// <summary>
        /// 处理拖拽事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// 处理结束拖拽事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;

                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }

                inventoryUI.UpdateSlotHightlight(-1);
            }
        }
    }
}
