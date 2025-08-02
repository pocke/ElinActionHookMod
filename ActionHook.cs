using System.Collections.Generic;
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

    static Dictionary<EventBase, List<HandlerBase>> Handlers { get; } = new Dictionary<EventBase, List<HandlerBase>>();

    public void Awake()
    {
        Instance = this;
        new Harmony(ModInfo.Guid).PatchAll();

        // TODO: Construct the list from a CSV file
        var ev = new Events.EnterZone() { ZoneType = Events.ZoneType.Nefia, Phase = Events.Phase.Before };
        var list = new List<HandlerBase>
        {
            new Handlers.Say(),
        };
        Handlers.Add(ev, list);
    }

    public static void Log(object message)
    {
        Instance.Logger.LogInfo(message);
    }

    public static void Call(EventBase ev)
    {
        Handlers.TryGetValue(ev, out var handlers);
        foreach (var handler in handlers)
        {
            handler.Handle(ev);
        }
    }
}
