using System;
using System.Collections.Generic;

namespace ActionHook;

public class Handlers
{
  public static readonly Dictionary<HandlerType, Type> EventTypeToClass = new()
  {
      { HandlerType.Say, typeof(Say) },
  };

  public enum HandlerType
  {
    Say,
  }

  public abstract class HandlerBase
  {
    public abstract HandlerType HandlerType { get; }
    public abstract void Handle(Events.EventBase ev);
  }



  public class Say : HandlerBase
  {
    public override HandlerType HandlerType => HandlerType.Say;

    public override void Handle(Events.EventBase ev)
    {
      Msg.SayRaw($"Event received: {ev} at phase {ev.Phase}");
    }
  }
}
