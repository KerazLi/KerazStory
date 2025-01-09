using System;
using System.Collections.Generic;


public static class EventHandle 
{
    public static event Action<InventoryLocation,List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> lists)
    {
        UpdateInventoryUI?.Invoke(location, lists);
    }
}
