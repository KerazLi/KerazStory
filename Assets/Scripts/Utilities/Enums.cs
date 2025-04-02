using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:Enums.cs
* @Author: Keraz
* @Description:枚举脚本
* @Date: 2025年02月19日 星期三 20:00:34
*/

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
    ChopTool,  // 用于砍伐树木
    BreakTool, // 用于破坏岩石
    WaterTool, // 用于浇水
    CollectTool, // 用于采集资源
    // 可收获的景观物品，用于增加游戏环境的丰富性
    ReapTool,
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
public enum PartType{
    None, Carry, Hoe, Break, Water, Chop, Collect, Reap
}
public enum PartName{
    Body,Hair,Arm,Tool
}

public enum Season
{
    春天,夏天,秋天,冬天
}

/// <summary>
/// 定义网格类型的枚举，用于表示游戏地图上不同功能的网格类型
/// </summary>
public enum GridType
{
    /// <summary>
    /// 可挖掘的网格，表示该网格内的地面可以被玩家挖掘
    /// </summary>
    Diggable,
    
    /// <summary>
    /// 掉落物品的网格，表示该网格上有物品可以被玩家拾取
    /// </summary>
    DropItem,
    
    /// <summary>
    /// 可放置家具的网格，表示玩家可以在该网格上放置家具
    /// </summary>
    PlaceFurniture,
    
    /// <summary>
    /// NPC障碍物网格，表示该网格被NPC占用，玩家不能通过
    /// </summary>
    NPCObstacle
}

public enum ParticaleEffectType
{
    None,LeavesFalling01,LeavesFalling02,Rock,ReapableScenery
}


