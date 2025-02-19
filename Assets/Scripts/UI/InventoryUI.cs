using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* @Program:InventoryUI.cs
* @Author: Keraz
* @Description:背包的UI
* @Date: 2025年02月19日 星期三 19:49:58
*/

namespace KFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemToolTip;
        [Header("拖拽图片")] 
        public Image dragItem;
        [Header("玩家背包")]
        [SerializeField] private GameObject bagUI;

        private bool bagOpen;
        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent+= OnBeforeSceneUnloadEvent;
        }
        

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHightlight(-1);
        }
        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent-= OnBeforeSceneUnloadEvent;
        }
        
        private void Start()
        {
            //给每个槽设置索引
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }

            bagOpen = bagUI.activeInHierarchy;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        /// <summary>
        /// 更新库存界面的事件处理方法。
        /// </summary>
        /// <param name="location">库存位置，可以是玩家背包或箱子。</param>
        /// <param name="lists">库存中的物品列表。</param>
        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> lists)
        {
            // 根据不同的库存位置更新界面
            switch (location)
            {
                // 当库存位置为玩家背包时
                case InventoryLocation.Player:
                    // 遍历玩家背包中的每个槽位
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        // 如果物品数量大于0，则更新槽位显示物品和数量
                        if (lists[i].ItemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(lists[i].itemID);
                            playerSlots[i].UpdateSlot(item, lists[i].ItemAmount);
                        }
                        else
                        {
                            // 如果物品数量为0，则更新槽位为空槽位
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
                // 当库存位置为箱子时，暂无操作
                case InventoryLocation.Box:
                    break;
                // 如果库存位置不在预期范围内，抛出异常
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }
        /// <summary>
        /// 打开背包UI
        /// </summary>
        public void OpenBagUI()
        {
            bagOpen = !bagOpen;
            bagUI.SetActive(bagOpen);
        }
        /// <summary>
        /// 更新装备栏高亮
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHightlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected&&slot.slotIndex==index)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
        }

    } 
}

