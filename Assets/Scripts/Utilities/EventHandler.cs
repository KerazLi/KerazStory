using System;
using System.Collections.Generic;
using UnityEngine;


public static class EventHandler 
{
    public static event Action<InventoryLocation,List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> lists)
    {
        UpdateInventoryUI?.Invoke(location, lists);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int ID, Vector3 position)
    {
        InstantiateItemInScene?.Invoke(ID, position);
    }
    public static event Action<ItemDetails,bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
}
