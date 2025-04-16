using MFrarm.CropPlant;
using UnityEngine;

/*
* @Program:Item.cs
* @Author: Keraz
* @Description:物品的详细信息
* @Date: 2025年02月19日 星期三 19:40:28
*/

namespace KFarm.Inventory
{
    
    public class Item : MonoBehaviour
    {
        /// <summary>
        /// 物品的唯一标识符。
        /// </summary>
        public int itemID;

        /// <summary>
        /// 用于渲染物品的精灵渲染器。
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 物品的碰撞器，用于物理交互。
        /// </summary>
        private BoxCollider2D coll;

        /// <summary>
        /// 物品的详细信息，如名称、描述等。
        /// </summary>
        public ItemDetails itemDetails;

        /// <summary>
        /// 初始化物品组件。
        /// </summary>
        private void Awake()
        {
            // 获取子对象上的精灵渲染器
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            // 获取当前对象上的2D盒碰撞器
            coll = GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// 在物品创建时进行初始化。
        /// </summary>
        private void Start()
        {
            // 如果物品ID非零，则初始化物品
            if (itemID != 0)
            {
                Init(itemID);
            }
        }

        /// <summary>
        /// 初始化物品的详细信息。
        /// </summary>
        /// <param name="ID">物品的唯一标识符。</param>
        public void Init(int ID)
        {
            itemID = ID;

            // 从InventoryManager中获取当前物品的数据
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails != null)
            {
                // 设置物品的显示精灵
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null
                    ? itemDetails.itemOnWorldSprite
                    : itemDetails.itemIcon;

                // 根据精灵渲染器的精灵调整碰撞器的大小和偏移
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }

            if (itemDetails.itemType==ItemType.ReapableScenery)
            {
                gameObject.AddComponent<ReapItem>();
                gameObject.GetComponent<ReapItem>().InitCropData(itemDetails.itemID);
                gameObject.AddComponent<ItemInteractive>();
            }
        }
    }
}
