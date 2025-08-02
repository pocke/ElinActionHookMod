using System;
using HarmonyLib;

namespace ActionHook;

[HarmonyPatch]
public static class PatchForEnterZone
{
  [HarmonyPrefix, HarmonyPatch(typeof(Player), nameof(Player.EnterLocalZone), new Type[] { typeof(Point), typeof(ZoneTransition), typeof(bool), typeof(Chara) })]
  public static void Player_EnterLocalZone_Prefix(Point p)
  {
    ActionHook.IsEnteringZone = true;

    p = p.Copy();
    if (EClass._zone.IsRegion)
    {
      p.Set(p.x + EClass.scene.elomap.minX, p.z + EClass.scene.elomap.minY);
    }

    Zone zone = EClass._zone.Region.GetZoneAt(p.x, p.z);
    var subType = zoneToSubType(zone);
    if (subType == null)
    {
      return;
    }

    var ev = new Events.EnterZone { SubType = subType, Phase = Events.Phase.Before };
    ActionHook.Call(ev);
  }

  [HarmonyPostfix, HarmonyPatch(typeof(Zone), nameof(Zone.Activate))]
  public static void Zone_Activate_Postfix(Zone __instance)
  {
    if (!ActionHook.IsEnteringZone)
    {
      return;
    }
    ActionHook.IsEnteringZone = false;

    var zone = __instance;
    var subType = zoneToSubType(zone);

    var ev = new Events.EnterZone { SubType = subType, Phase = Events.Phase.After };
    ActionHook.Call(ev);
  }

  static Events.SubType? zoneToSubType(Zone zone)
  {
    if (zone is Zone_Dungeon)
    {
      return Events.SubType.Nefia;
    }
    else if (zone != null && zone.IsPCFaction)
    {
      return Events.SubType.PCFaction;
    }
    else
    {
      return null;
    }
  }
}

[HarmonyPatch]
public static class PatchForSleep
{
  [HarmonyPrefix, HarmonyPatch(typeof(HotItemActionSleep), nameof(HotItemActionSleep.Perform))]
  public static void HotItemActionSleep_Perform_Prefix()
  {
    var ev = new Events.Sleep { Phase = Events.Phase.Before };
    ActionHook.Call(ev);
  }
}

[HarmonyPatch]
public static class PatchForStairs
{
  [HarmonyPrefix, HarmonyPatch(typeof(TraitNewZone), nameof(TraitNewZone.MoveZone))]
  public static void TraitNewZone_MoveZone_Prefix(TraitNewZone __instance)
  {
    ActionHook.IsEnteringZone = true;

    if (__instance is TraitStairsDown)
    {
      var ev = new Events.GoDownStairs { Phase = Events.Phase.Before };
      ActionHook.Call(ev);
    }
    else if (__instance is TraitStairsUp)
    {
      var ev = new Events.GoUpStairs { Phase = Events.Phase.Before };
      ActionHook.Call(ev);
    }
  }
}

[HarmonyPatch]
public static class PatchForStartCrafting
{
  [HarmonyPrefix, HarmonyPatch(typeof(LayerCraft), nameof(LayerCraft.SetFactory))]
  public static void LayerCraft_SetFactory_Prefix(Thing t)
  {
    triggerEvent(t);
  }

  [HarmonyPrefix, HarmonyPatch(typeof(LayerDragGrid), nameof(LayerDragGrid.CreateCraft))]
  public static void LayerDragGrid_CreateCraft_Prefix(TraitCrafter crafter)
  {
    var t = crafter.owner;
    triggerEvent(t);
  }

  static void triggerEvent(Card t)
  {
    var skill = t.trait.GetParam(1) ?? "handicraft";
    skill = char.ToUpper(skill[0]) + skill.Substring(1);
    if (!Enum.TryParse(skill, out Events.SubType subType))
    {
      ActionHook.Log($"Unknown skill: {skill}");
      return;
    }

    var ev = new Events.StartCrafting { SubType = subType, Phase = Events.Phase.Before };
    ActionHook.Call(ev);
  }
}
