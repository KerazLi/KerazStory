using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            EventHandle.UpdateInventoryUI += OnUpdateInventoryUI;
        }
        private void OnDisable()
        {
            EventHandle.UpdateInventoryUI -= OnUpdateInventoryUI;
        }
        
        private void Start()
        {
            //给每个槽设置索引
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
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
        
    } 
}

