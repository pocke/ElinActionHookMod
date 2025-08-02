using System.Collections.Generic;
using System.IO;
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

    Dictionary<Events.EventBase, List<Actions.ActionBase>> Actions { get; set; } = null;

    public void Awake()
    {
        Instance = this;
        new Harmony(ModInfo.Guid).PatchAll();

        Actions = ConfigLoader.LoadActionsFromCsv(ConfigLoader.ConfigPath);
    }

    public static void Log(object message)
    {
        Instance.Logger.LogInfo(message);
    }

    public static void Call(Events.EventBase ev)
    {
        Instance.Actions.TryGetValue(ev, out var actions);
        foreach (var action in actions)
        {
            action.Do(ev);
        }
    }
}
