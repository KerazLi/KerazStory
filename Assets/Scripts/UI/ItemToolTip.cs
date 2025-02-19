using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* @Program:ItemToolTip.cs
* @Author: Keraz
* @Description:物品提示框脚本
* @Date: 2025年02月19日 星期三 19:53:49
*/

public class ItemToolTip : MonoBehaviour
{
    // 用于显示物品名称的文本控件
    [SerializeField] private TextMeshProUGUI nameText;
    // 用于显示物品类型的文本控件
    [SerializeField] private TextMeshProUGUI typeText;
    // 用于显示物品描述的文本控件
    [SerializeField] private TextMeshProUGUI descriptionText;
    // 用于显示物品价值的文本控件
    [SerializeField] private Text valueText;
    // 包含tooltip下半部分的Game Object，根据物品类型显示或隐藏
    [SerializeField] private GameObject bottomPart;
    
    /// <summary>
    /// 设置tooltip的内容。
    /// </summary>
    /// <param name="itemDetails">包含物品详细信息的数据结构。</param>
    /// <param name="slotType">物品所在的槽类型，影响物品价值的显示。</param>
    public void SetupTooltip(ItemDetails itemDetails, SlotType slotType)
    {
        // 设置物品名称
        nameText.text = itemDetails.itemName;
    
        // 设置物品类型
        typeText.text = GetItemType(itemDetails.itemType);
    
        // 设置物品描述
        descriptionText.text = itemDetails.itemDescription;
    
        // 根据物品类型决定是否显示tooltip的下半部分以及如何显示物品价值
        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);
    
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)
            {
                // 如果物品在背包中，根据卖出百分比调整价格
                price = (int)(price * itemDetails.sellPercentage);
            }
    
            // 显示物品价值
            valueText.text = price.ToString();
        }
        else
        {
            // 对于其他类型的物品，隐藏tooltip的下半部分
            bottomPart.SetActive(false);
        }
    
        // 强制重新构建布局，以适应tooltip内容的变化
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    
    /// <summary>
    /// 根据物品类型获取物品类型的显示字符串。
    /// </summary>
    /// <param name="itemType">物品类型。</param>
    /// <returns>物品类型的显示字符串。</returns>
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "工具",
            ItemType.ChopTool => "工具",
            ItemType.CollectTool => "工具",
            ItemType.HoeTool => "工具",
            ItemType.ReapTool => "工具",
            ItemType.WaterTool => "工具",
            _ => "无"
        };
    }
}
