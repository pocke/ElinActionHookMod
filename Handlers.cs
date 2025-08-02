using System;
using System.Collections.Generic;

namespace ActionHook;

public class Handlers
{
  public static readonly Dictionary<HandlerType, Type> EventTypeToClass = new()
  {
      { HandlerType.ShowMessage, typeof(ShowMessage) },
  };

  public enum HandlerType
  {
    ShowMessage,
  }

  public abstract class HandlerBase
  {
    public abstract HandlerType HandlerType { get; }
    public abstract void Handle(Events.EventBase ev);
  }



  public class ShowMessage : HandlerBase
  {
    public override HandlerType HandlerType => HandlerType.ShowMessage;

    public override void Handle(Events.EventBase ev)
    {
      EClass.ui.Say($"Event received: {ev} at phase {ev.Phase}");
    }
  }
}
