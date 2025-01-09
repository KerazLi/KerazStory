using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定义游戏中的物品类型，包括种子、商品、工具等
public enum ItemType
{
    // 种植类物品，例如各种种子
    Seed,
    // 可交易的商品，通常是资源或产品
    Commodity,
    // 用于装饰玩家家园或环境的家具
    Furniture,
    // 工具类别，包含农耕和采集资源所需的各种工具
    HoeTool,   // 用于耕地
    ChopToop,  // 用于砍伐树木
    BreakTool, // 用于破坏岩石
    WaterTool, // 用于浇水
    CollectTool, // 用于采集资源
    // 可收获的景观物品，用于增加游戏环境的丰富性
    ReapableScenery
}

public enum SlotType
{
    Bag,Box,Shop
}

public enum InventoryLocation
{
    Player,Box
}

