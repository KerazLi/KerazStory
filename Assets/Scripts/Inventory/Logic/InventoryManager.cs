using UnityEngine;

namespace Inventory.Logic
{
    /// <summary>
    /// 背包物品管理函数
    /// </summary>
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;
        public InventoryBag_SO playerBag;
        
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(x => x.itemID == ID);
        }
        public void AddItem(Item item,bool toDestory)
        {
            //背包是否有空位
            //是否已经有该物品
            InventoryItem newItem = new();
            newItem.itemID = item.itemID;
            newItem.ItemAmount = 1;
            playerBag.itemList[0] = newItem;
            Debug.Log(GetItemDetails(item.itemID).itemID+"Name"+GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
        }
    } 
}

