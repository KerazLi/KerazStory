using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:ItemPickUp.cs
* @Author: Keraz
* @Description:拾取功能
* @Date: 2025年02月19日 星期三 19:44:31
*/

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
