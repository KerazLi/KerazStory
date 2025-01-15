using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> lists)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (lists[i].ItemAmount>0)
                        {
                            var item=InventoryManager.Instance.GetItemDetails(lists[i].itemID);
                            playerSlots[i].UpdateSlot(item,lists[i].ItemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
                case InventoryLocation.Box:
                    break;
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

