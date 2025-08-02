using System;
using HarmonyLib;

namespace ActionHook;

[HarmonyPatch]
public static class Patch
{
    [HarmonyPrefix, HarmonyPatch(typeof(Player),nameof(Player.EnterLocalZone), new Type[] { typeof(Point), typeof(ZoneTransition), typeof(bool), typeof(Chara) })]
    public static void Player_EnterLocalZone_Prefix(Player __instance, Point p)
    {
		Zone zone = EClass._zone.Region.GetZoneAt(p.x, p.z);
        Events.ZoneType t;

        if (zone is Zone_Dungeon) {
            t = Events.ZoneType.Nefia;
        } else {
            return;
        }

        var ev = new Events.EnterZone { ZoneType = t };
        ActionHook.Call(ev);
    }
}
