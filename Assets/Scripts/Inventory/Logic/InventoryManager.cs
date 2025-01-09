using UnityEngine;

namespace Inventory.Logic
{
    /// <summary>
    /// 背包物品管理函数
    /// 
    /// </summary>
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;
        
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(x => x.itemID == ID);
        }
        public void AddItem(Item item,bool toDestory)
        {
            Debug.Log(GetItemDetails(item.itemID).itemID+"Name"+GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
        }
    } 
}

