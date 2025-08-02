using System;
using HarmonyLib;

namespace ActionHook;

[HarmonyPatch]
public static class Patch
{
    [HarmonyPrefix, HarmonyPatch(typeof(Player), nameof(Player.EnterLocalZone), new Type[] { typeof(Point), typeof(ZoneTransition), typeof(bool), typeof(Chara) })]
    public static void Player_EnterLocalZone_Prefix(Point p, out Events.SubType? __state)
    {
        p = p.Copy();
        if (EClass._zone.IsRegion)
        {
            p.Set(p.x + EClass.scene.elomap.minX, p.z + EClass.scene.elomap.minY);
        }

        Zone zone = EClass._zone.Region.GetZoneAt(p.x, p.z);
        __state = null;

        if (zone is Zone_Dungeon)
        {
            __state = Events.SubType.Nefia;
        }
        else if (zone != null && zone.IsPCFaction) {
            __state = Events.SubType.PCFaction;
        }
        else
        {
            return;
        }

        var ev = new Events.EnterZone { SubType = __state, Phase = Events.Phase.Before };
        ActionHook.Call(ev);
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Player), nameof(Player.EnterLocalZone), new Type[] { typeof(Point), typeof(ZoneTransition), typeof(bool), typeof(Chara) })]
    public static void Player_EnterLocalZone_Postfix(Point p, Events.SubType? __state)
    {
        if (__state == null)
        {
            return;
        }
        var ev = new Events.EnterZone { SubType = __state, Phase = Events.Phase.After };
        ActionHook.Call(ev);
    }
}
