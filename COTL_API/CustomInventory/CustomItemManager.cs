using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;
using System.Linq;
using UnityEngine;

namespace COTL_API.CustomInventory;

public static partial class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> CustomItems { get; } = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        InventoryItem.ITEM_TYPE itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, item.InternalName);
        item.ItemType = itemType;
        item.ModPrefix = guid;
        item.InternalObjectName = $"CustomItem_{item.InternalName}";

        CustomItems.Add(itemType, item);

        return itemType;
    }

    /// <summary>
    /// Used to define an items rarity for the purpose of adding them to offering shrines.
    /// </summary>
    public enum ItemRarity
    {
        COMMON,
        RARE
    }

    /// <summary>
    /// A method to return whether to drop loot or not based on the custom items chances to drop.
    /// </summary>
    /// <param name="customInventoryItem">The </param>
    /// <returns>Returns true/false based on the roll.</returns>
    public static bool DropLoot(CustomInventoryItem customInventoryItem)
    {
        float roll = Random.Range(0, 101);
        float chance = customInventoryItem.DungeonChestSpawnChance + customInventoryItem.DungeonChestSpawnChance * DataManager.Instance.GetLuckMultiplier();
        if (Plugin.Debug) Plugin.Logger.LogDebug($"{customInventoryItem.InternalObjectName} Roll/Chance: {roll} / {chance}: Win? {roll <= chance}");
        return roll <= chance;
        //return true;
    }

    /// <summary>
    /// Used to retrieve the custom item from the custom item dictionary based on it's internal object name.
    /// </summary>
    /// <param name="name">Name of the items internal object to search for.</param>
    /// <returns>If found, returns the CustomInventoryItem object.</returns>
    public static KeyValuePair<InventoryItem.ITEM_TYPE, CustomInventoryItem> GetItemObjectByInternalObjectName(string name)
    {
        return (from item in CustomItems where item.Value.InternalObjectName == name select item).FirstOrDefault();
    }

    public static void AddGift(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Add(item);
    }

    public static void RemoveGift(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllGifts.Contains(item)) DataManager.AllGifts.Remove(item);
    }

    public static void AddNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (!DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Add(item);
    }

    public static void RemoveNecklace(InventoryItem.ITEM_TYPE item)
    {
        if (DataManager.AllNecklaces.Contains(item)) DataManager.AllNecklaces.Remove(item);
    }
}