using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ActionHook;

public class ConfigLoader
{
  public class ActionRecord
  {
    public Events.EventType EventType { get; set; }
    public Events.SubType? SubType { get; set; }
    public Events.Phase Phase { get; set; }
    public Actions.ActionType ActionType { get; set; }
    public string[] ActionArgs { get; set; }
  }

  public class ActionRecordMap : ClassMap<ActionRecord>
  {
    public ActionRecordMap()
    {
      Map(m => m.EventType).Name("EventType");
      Map(m => m.SubType).Name("SubType");
      Map(m => m.Phase).Name("Phase");
      Map(m => m.ActionType).Name("ActionType");
      Map(m => m.ActionArgs).Name("ActionArgs").ConvertUsing(row =>
      {
        var args = row.GetField<string>("ActionArgs");
        return string.IsNullOrEmpty(args) ? Array.Empty<string>() : args.Split('/');
      });
    }
  }

  public static string ConfigPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hooks.csv");

  public static Dictionary<Events.EventBase, List<Actions.ActionBase>> LoadActionsFromCsv(string filePath)
  {

    if (!File.Exists(filePath))
    {
      ActionHook.DisplayError($"Config file not found: {filePath}");
      return emptyActions();
    }

    List<ActionRecord> records;
    try
    {
      using var reader = new StreamReader(filePath);
      using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
      csv.Configuration.RegisterClassMap<ActionRecordMap>();
      records = csv.GetRecords<ActionRecord>().ToList();
    }
    catch (CsvHelperException ex)
    {
      ActionHook.DisplayError($"Error on reading config file: {ex.Message}");
      return emptyActions();
    }

    var actionsDict = emptyActions();
    try
    {
      foreach (var record in records)
      {
        var evKlass = Events.EventTypeToClass[record.EventType];
        var ev = (Events.EventBase)Activator.CreateInstance(evKlass);
        ev.SubType = record.SubType;
        ev.Phase = record.Phase;

        var actionKlass = Actions.EventTypeToClass[record.ActionType];
        var action = (Actions.ActionBase)Activator.CreateInstance(actionKlass);
        action.ActionArgs = record.ActionArgs;
        action.Validate();

        if (!actionsDict.ContainsKey(ev))
        {
          actionsDict[ev] = new List<Actions.ActionBase>();
        }
        actionsDict[ev].Add(action);
      }
    }
    catch (ValidationException ex)
    {
      ActionHook.DisplayError($"Error on validating config file: {ex.Message}");
      return emptyActions();
    }

    return actionsDict;
  }

  static Dictionary<Events.EventBase, List<Actions.ActionBase>> emptyActions()  => new();
}
