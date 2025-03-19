using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

 /*
* @Program:EventHandler.cs
* @Author: Keraz
* @Description:广播的一些类
* @Date: 2025年02月19日 星期三 20:01:32
*/

public static class EventHandler 
{
    // 定义一个事件，用于更新库存UI
    // 该事件的处理程序将接收一个InventoryLocation对象和一个InventoryItem列表作为参数
    public static event Action<InventoryLocation,List<InventoryItem>> UpdateInventoryUI;

    /// <summary>
    /// 调用UpdateInventoryUI事件来更新库存UI
    /// </summary>
    /// <param name="location">库存位置</param>
    /// <param name="lists">库存物品列表</param>
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> lists)
    {
        UpdateInventoryUI?.Invoke(location, lists);
    }

    // 定义一个事件，用于在场景中实例化物品
    // 该事件的处理程序将接收一个物品ID和一个位置向量作为参数
    public static event Action<int, Vector3> InstantiateItemInScene;

    /// <summary>
    /// 调用InstantiateItemInScene事件，在场景中实例化指定物品
    /// </summary>
    /// <param name="ID">物品ID</param>
    /// <param name="position">物品实例化的位置</param>
    public static void CallInstantiateItemInScene(int ID, Vector3 position)
    {
        InstantiateItemInScene?.Invoke(ID, position);
    }

    public static event Action<int, Vector3,ItemType> DropItemEvent;
    public static void CallDropItemEvent(int ID, Vector3 position,ItemType itemType)
    {
        DropItemEvent?.Invoke(ID, position,itemType);
    }

    // 定义一个事件，用于响应物品选择状态的变化
    // 该事件的处理程序将接收一个ItemDetails对象和一个布尔值作为参数，表示物品详情和是否选中
    public static event Action<ItemDetails,bool> ItemSelectedEvent;

    /// <summary>
    /// 调用ItemSelectedEvent事件，通知观察者物品选择状态发生变化
    /// </summary>
    /// <param name="itemDetails">物品详情</param>
    /// <param name="isSelected">物品是否被选中</param>
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    // 定义一个事件，用于每过一分钟游戏时间就通知观察者
    // 该事件的处理程序将接收两个整数作为参数，表示当前游戏时间的分钟和小时
    public static event Action<int,int> GameMinuteEvent;

    /// <summary>
    /// 调用GameMinuteEvent事件，通知观察者游戏时间每过一分钟
    /// </summary>
    /// <param name="min">当前游戏时间的分钟</param>
    /// <param name="hour">当前游戏时间的小时</param>
    public static void CallGameMinuteEvent(int min, int hour)
    {
        GameMinuteEvent?.Invoke(min, hour);
    }

    public static event Action<int, Season> GameDayEvent;
    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }

    // 定义一个事件，用于通知观察者游戏数据发生变化
    // 该事件的处理程序将接收小时、天、月、年和季节作为参数
    public static event Action<int,int,int,int,Season> GameDataEvent;

    /// <summary>
    /// 调用GameDataEvent事件，通知观察者游戏数据发生变化
    /// </summary>
    /// <param name="hour">当前游戏时间的小时</param>
    /// <param name="day">当前游戏时间的天</param>
    /// <param name="month">当前游戏时间的月</param>
    /// <param name="year">当前游戏时间的年</param>
    /// <param name="season">当前游戏时间的季节</param>
    public static void CallGameDataEvent(int hour, int day,  int month,int year, Season season)
    {
        GameDataEvent?.Invoke(hour, day,  month,year, season);
    }

    // 定义一个事件，用于在场景转换时通知观察者
    // 该事件的处理程序将接收一个场景名称和一个位置向量作为参数，表示下一个场景的名称和玩家位置
    public static  event Action<string,Vector3> TransitionEvent;

    /// <summary>
    /// 调用TransitionEvent事件，通知观察者即将进行场景转换
    /// </summary>
    /// <param name="sceneName">下一个场景的名称</param>
    /// <param name="position">玩家在下一个场景的位置</param>
    public static void CallTransitionEvent(string sceneName, Vector3 position)
    {
        TransitionEvent?.Invoke(sceneName, position);
    }

    // 定义一个事件，用于在卸载场景前通知观察者
    public static event Action BeforeSceneUnloadEvent;

    /// <summary>
    /// 调用BeforeSceneUnloadEvent事件，通知观察者当前场景即将卸载
    /// </summary>
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    // 定义一个事件，用于在加载场景后通知观察者
    public static event Action AfterSceneLoadEvent;

    /// <summary>
    /// 调用AfterSceneLoadEvent事件，通知观察者新场景已加载完成
    /// </summary>
    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }
    /// <summary>
    /// 定义一个静态事件，当对象需要移动到特定位置时触发。
    /// </summary>
    /// <remarks>
    /// 该事件允许订阅者响应对象的移动事件，从而同步其位置。
    /// </remarks>
    public static event Action<Vector3> MoveToPosition;
    
    /// <summary>
    /// 静态方法，用于触发MoveToPosition事件。
    /// </summary>
    /// <param name="position">对象需要移动到的三维位置。</param>
    /// <remarks>
    /// 该方法确保只有当有订阅者时才调用事件，避免了空引用异常。
    /// </remarks>
    public static void CallMoveToPosition(Vector3 position)
    {
        MoveToPosition?.Invoke(position);
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 position, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(position, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
    public static void CallExecuteActionAfterAnimation(Vector3 position, ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(position, itemDetails);
    }
    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int ID, TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(ID, tileDetails);
    }

}
