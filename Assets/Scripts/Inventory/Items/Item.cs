using System.Collections;
using System.Collections.Generic;
using Inventory.Logic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private ItemDetails itemDetails;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (itemID != 0)
        {
            Init(itemID);
        }
    }

    public void Init(int ID)
    {
        itemID = ID;

        //Inventory获得当前数据
        itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

        if (itemDetails != null)
        {
            spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null
                ? itemDetails.itemOnWorldSprite
                : itemDetails.itemIcon;

            // 根据精灵渲染器的精灵调整碰撞器的大小和偏移
            Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
            coll.size = newSize;
            coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
        }
    }
}
