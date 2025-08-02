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
  public class HandlerRecord
  {
    public Events.EventType EventType { get; set; }
    public Events.SubType? SubType { get; set; }
    public Events.Phase Phase { get; set; }
    public Handlers.HandlerType HandlerType { get; set; }
  }

  public static string ConfigPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hooks.csv");

  public static Dictionary<Events.EventBase, List<Handlers.HandlerBase>> LoadHandlersFromCsv(string filePath)
  {
    if (!File.Exists(filePath))
    {
      ActionHook.Log($"Config file not found: {filePath}");
      return new Dictionary<Events.EventBase, List<Handlers.HandlerBase>>();
    }

    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    var records = csv.GetRecords<HandlerRecord>().ToList();

    var handlers = new Dictionary<Events.EventBase, List<Handlers.HandlerBase>>();
    foreach (var record in records)
    {
      var evKlass = Events.EventTypeToClass[record.EventType];
      var ev = (Events.EventBase)Activator.CreateInstance(evKlass);
      ev.SubType = record.SubType;
      ev.Phase = record.Phase;

      var handlerKlass = Handlers.EventTypeToClass[record.HandlerType];
      var handler = (Handlers.HandlerBase)Activator.CreateInstance(handlerKlass);

      if (!handlers.ContainsKey(ev))
      {
        handlers[ev] = new List<Handlers.HandlerBase>();
      }
      handlers[ev].Add(handler);
    }

    return handlers;
  }
}
