using System;
using HarmonyLib;

namespace ActionHook;

[HarmonyPatch]
public static class PatchForEnterZone
{
  [HarmonyPrefix, HarmonyPatch(typeof(Player), nameof(Player.EnterLocalZone), new Type[] { typeof(Point), typeof(ZoneTransition), typeof(bool), typeof(Chara) })]
  public static void Player_EnterLocalZone_Prefix(Point p)
  {
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
    var zone = __instance;
    var subType = zoneToSubType(zone);

    // This is called after the zone is activated, so we use After phase.
    // The event will be handled in the next frame.
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
