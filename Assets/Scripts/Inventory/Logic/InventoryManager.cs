using System;
using UnityEngine;

namespace KFarm.Inventory
{
    /// <summary>
    /// 背包物品管理函数
    /// </summary>
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;
        public InventoryBag_SO playerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }

        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(x => x.itemID == ID);
        }
        
        /// <summary>
        /// 世界中拾取物体
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="toDestory">世界物品是否消失</param>
        public void AddItem(Item item,bool toDestory)
        {
            //背包是否有空位
            var index= GetItemIndexInBag(item.itemID);
            AddItemAtindex(item.itemID,index,1);
            
            //是否已经有该物品
            /*InventoryItem newItem = new();
            newItem.itemID = item.itemID;
            newItem.ItemAmount = 1;
            playerBag.itemList[0] = newItem;*/
            Debug.Log(GetItemDetails(item.itemID).itemID+"Name"+GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.itemList);
        }
        
        /// <summary>
        /// 检查背包是否还有空位
        /// </summary>
        /// <returns></returns>
        public bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID==0)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 获取物品是否在背包中
        /// </summary>
        /// <param name="Id">物品ID</param>
        /// <returns>-1则没有物品</returns>
        private int GetItemIndexInBag(int Id)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID==Id)
                {
                    return i;
                }
            }

            return -1;
        }
        /// <summary>
        /// 在指定背包序号里添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">序号</param>
        /// <param name="amount">数量</param>
        private void AddItemAtindex(int ID,int index,int amount)
        {
            if (index==-1&&CheckBagCapacity())//物品不存在背包中
            {
                var item = new InventoryItem{itemID = ID,ItemAmount = amount};
                for (int i=0;i<playerBag.itemList.Count;i++) 
                {
                    if (playerBag.itemList[i].itemID==0)
                    {
                        playerBag.itemList[i]=item;
                        break;
                    }   
                }
            }
            else//存在于背包中
            {
                int currentAmount=playerBag.itemList[index].ItemAmount+amount;
                var item = new InventoryItem { itemID = ID, ItemAmount = currentAmount};
                playerBag.itemList[index] = item;
            }
        }
        /// <summary>
        /// Player背包范围内交换物品
        /// </summary>
        /// <param name="fromIndex">起始序号</param>
        /// <param name="targetIndex">目标数据序号</param>
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID != 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[fromIndex] = new InventoryItem();
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    } 
}

