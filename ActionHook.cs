using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ActionHook;

internal static class ModInfo
{
    internal const string Guid = "me.pocke.action-hook";
    internal const string Name = "Action Hook";
    internal const string Version = "1.0.0";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class ActionHook : BaseUnityPlugin
{
    internal static ActionHook Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        new Harmony("ActionHook").PatchAll();
    }

    public static void Log(object message)
    {
        Instance.Logger.LogInfo(message);
    }
}
