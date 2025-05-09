using UnityEngine;

/*
* @Program:DataCollection.cs
* @Author: Keraz
* @Description:一些数据收集类
* @Date: 2025年02月19日 星期三 20:00:18
*/

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0, 1)]
    public float sellPercentage;
}
[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int ItemAmount;
}
[System.Serializable]
public class AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

/// <summary>
/// 用于序列化的三维向量类，适用于将 Vector3 数据进行序列化。
/// </summary>
[System.Serializable]
public class SerializableVector3
{
    /// <summary>
    /// 三维向量的 x 坐标。
    /// </summary>
    public float x;

    /// <summary>
    /// 三维向量的 y 坐标。
    /// </summary>
    public float y;

    /// <summary>
    /// 三维向量的 z 坐标。
    /// </summary>
    public float z;

    /// <summary>
    /// 初始化 SerializableVector3 类的新实例。
    /// </summary>
    /// <param name="vector3">要序列化的 Vector3 对象。</param>
    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    /// <summary>
    /// 将当前实例转换为 Vector3 对象。
    /// </summary>
    /// <returns>表示当前实例的 Vector3 对象。</returns>
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 将当前实例转换为 Vector2Int 对象，x 和 y 坐标会被强制转换为整数。
    /// </summary>
    /// <returns>表示当前实例的 Vector2Int 对象。</returns>
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}


[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}
[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;
    public int daysSinceDug=-1;
    public int daysSinceWatered=-1;
    public int seedItemID = -1;
    public int growthDays = -1;
    public int daysSinceLastHarvest = -1;
}

[System.Serializable]
public class NPCPosition
{
    public Transform npc;
    public string startScene;
    public Vector3 position;
}