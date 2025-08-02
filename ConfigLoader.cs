using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;

namespace ActionHook;

public class ConfigLoader
{
public class ActionRecord
  {
    public Events.EventType EventType { get; set; }
    public Events.SubType? SubType { get; set; }
    public Events.Phase Phase { get; set; }
   public Actions.ActionType ActionType { get; set; }
  }

  public static string ConfigPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hooks.csv");

  public static Dictionary<Events.EventBase, List<Actions.ActionBase>> LoadActionsFromCsv(string filePath)
  {
    if (!File.Exists(filePath))
    {
      ActionHook.Log($"Config file not found: {filePath}");
      return new Dictionary<Events.EventBase, List<Actions.ActionBase>>();
    }

    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    var records = csv.GetRecords<ActionRecord>().ToList();

    var actionsDict = new Dictionary<Events.EventBase, List<Actions.ActionBase>>();
    foreach (var record in records)
    {
      var evKlass = Events.EventTypeToClass[record.EventType];
      var ev = (Events.EventBase)Activator.CreateInstance(evKlass);
      ev.SubType = record.SubType;
      ev.Phase = record.Phase;

      var actionKlass = global::ActionHook.Actions.EventTypeToClass[record.ActionType];
      var action = (global::ActionHook.Actions.ActionBase)Activator.CreateInstance(actionKlass);

      if (!actionsDict.ContainsKey(ev))
      {
        actionsDict[ev] = new List<Actions.ActionBase>();
      }
      actionsDict[ev].Add(action);
    }

    return actionsDict;
  }
}
