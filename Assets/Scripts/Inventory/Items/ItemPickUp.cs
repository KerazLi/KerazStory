using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (item.itemDetails.canPickedup)
                {
                    //拾取物添加到背包
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }
    }
}
