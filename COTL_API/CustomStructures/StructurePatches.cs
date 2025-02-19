﻿using System.Collections.Generic;
using Lamb.UI.BuildMenu;
using System.Linq;
using HarmonyLib;
using System;

namespace COTL_API.CustomStructures;

public partial class CustomStructureManager
{
    [HarmonyPatch(typeof(StructureBrain), "CreateBrain")]
    [HarmonyPrefix]
    private static bool StructureBrain_CreateBrain(ref StructureBrain __result, StructuresData data)
    {
        if (!CustomStructures.ContainsKey(data.Type)) return true;
        StructureBrain structureBrain = new StructureBrain();
        StructureBrain.ApplyConfigToData(data);
        structureBrain.Init(data);
        StructureBrain._brainsByID.Add(data.ID, structureBrain);
        StructureManager.StructuresAtLocation(data.Location).Add(structureBrain);
        __result = structureBrain;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetInfoByType")]
    [HarmonyPrefix]
    private static bool StructuresData_GetInfoByType(ref StructuresData __result, StructureBrain.TYPES Type, int variantIndex)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].StructuresData;
        
        if (__result.PrefabPath == null) __result.PrefabPath = CustomStructures[Type].PrefabPath;
        
        __result.Type = Type;
        __result.VariantIndex = variantIndex;
        __result.IsUpgrade = StructuresData.IsUpgradeStructure(__result.Type);
        __result.UpgradeFromType = StructuresData.GetUpgradePrerequisite(__result.Type);
        
        return false;
    }

    [HarmonyPatch(typeof(FollowerCategory), "GetStructuresForCategory")]
    [HarmonyPostfix]
    private static void FollowerCategory_GetStructuresForCategory(ref List<StructureBrain.TYPES> __result, FollowerCategory.Category category)
    {
        __result.AddRange(from structure in CustomStructures.Values where structure.Category == category select structure.StructureType);
    }

    [HarmonyPatch(typeof(StructuresData), "GetUnlocked")]
    [HarmonyPrefix]
    private static void StructuresData_GetUnlocked(StructureBrain.TYPES Types)
    {
        if (!CustomStructures.ContainsKey(Types)) return;
        if (!DataManager.Instance.UnlockedStructures.Contains(Types)) DataManager.Instance.UnlockedStructures.Add(Types);
        if (!DataManager.Instance.RevealedStructures.Contains(Types)) DataManager.Instance.RevealedStructures.Add(Types);
    }

    [HarmonyPatch(typeof(TypeAndPlacementObjects), "GetByType")]
    [HarmonyPrefix]
    private static bool TypeAndPlacementObjects_GetByType(ref TypeAndPlacementObject __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetTypeAndPlacementObject();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "BuildDurationGameMinutes")]
    [HarmonyPrefix]
    private static bool StructuresData_BuildDurationGameMinutes(ref int __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].BuildDurationMinutes;
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetCost")]
    [HarmonyPrefix]
    private static bool StructuresData_GetCost(ref List<StructuresData.ItemCost> __result, StructureBrain.TYPES Type)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].Cost;
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetLocalizedNameStatic", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedNameStatic(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetLocalizedNameStatic();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "LocalizedName", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedName(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].LocalizedName();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "LocalizedDescription", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedDescription(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].LocalizedDescription();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "LocalizedPros", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedPros(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].LocalizedPros();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "LocalizedCons", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_LocalizedCons(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].LocalizedCons();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetLocalizedName", new Type[] {  })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedName(StructuresData __instance, ref string __result)
    {
        if (!CustomStructures.ContainsKey(__instance.Type)) return true;
        __result = CustomStructures[__instance.Type].GetLocalizedName();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetLocalizedDescription", new Type[] {  })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedDescription(StructuresData __instance, ref string __result)
    {
        if (!CustomStructures.ContainsKey(__instance.Type)) return true;
        __result = CustomStructures[__instance.Type].GetLocalizedDescription();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetLocalizedLore", new Type[] {  })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedLore(StructuresData __instance, ref string __result)
    {
        if (!CustomStructures.ContainsKey(__instance.Type)) return true;
        __result = CustomStructures[__instance.Type].GetLocalizedLore();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetLocalizedName", new Type[] { typeof(bool), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    public static bool StructuresData_GetLocalizedName(StructuresData __instance, bool plural, bool withArticle,
        bool definite, ref string __result)
    {
        if (!CustomStructures.ContainsKey(__instance.Type)) return true;
        __result = CustomStructures[__instance.Type].GetLocalizedName(plural, withArticle, definite);
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "GetResearchCost", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_GetResearchCost(StructureBrain.TYPES Type, ref int __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetResearchCost();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "RequiresTempleToBuild", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_RequiresTempleToBuild(StructureBrain.TYPES type, ref bool __result)
    {
        if (!CustomStructures.ContainsKey(type)) return true;
        __result = CustomStructures[type].RequiresTempleToBuild();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetBuildOnlyOne", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_GetBuildOnlyOne(StructureBrain.TYPES Type, ref bool __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetBuildOnlyOne();
        return false;
    }
    
    [HarmonyPatch(typeof(StructuresData), "GetBuildSfx", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_GetBuildSfx(StructureBrain.TYPES Type, ref string __result)
    {
        if (!CustomStructures.ContainsKey(Type)) return true;
        __result = CustomStructures[Type].GetBuildSfx();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "HiddenUntilUnlocked", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_HiddenUntilUnlocked(StructureBrain.TYPES structure, ref bool __result)
    {
        if (!CustomStructures.ContainsKey(structure)) return true;
        __result = CustomStructures[structure].HiddenUntilUnlocked();
        return false;
    }

    [HarmonyPatch(typeof(StructuresData), "CanBeFlipped", new Type[] { typeof(StructureBrain.TYPES) })]
    [HarmonyPrefix]
    public static bool StructuresData_CanBeFlipped(StructureBrain.TYPES type, ref bool __result)
    {
        if (!CustomStructures.ContainsKey(type)) return true;
        __result = CustomStructures[type].CanBeFlipped();
        return false;
    }
}