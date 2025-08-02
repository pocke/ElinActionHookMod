using System.Collections.Generic;
using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using static ActionHook.Handlers;

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

    Dictionary<Events.EventBase, List<Handlers.HandlerBase>> Handlers { get; set; } = null;

    public void Awake()
    {
        Instance = this;
        new Harmony(ModInfo.Guid).PatchAll();

        Handlers = ConfigLoader.LoadHandlersFromCsv(ConfigLoader.ConfigPath);
    }

    public static void Log(object message)
    {
        Instance.Logger.LogInfo(message);
    }

    public static void Call(Events.EventBase ev)
    {
        Instance.Handlers.TryGetValue(ev, out var handlers);
        foreach (var handler in handlers)
        {
            handler.Handle(ev);
        }
    }
}
