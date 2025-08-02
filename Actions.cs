using System;
using System.Collections.Generic;

namespace ActionHook;

public static class Actions
{
  public static readonly Dictionary<ActionType, Type> EventTypeToClass = new()
  {
      { ActionType.ShowMessage, typeof(ShowMessage) },
  };

  public enum ActionType
  {
    ShowMessage,
  }

  public abstract class ActionBase
  {
    public abstract ActionType ActionType { get; }
    public abstract void Do(Events.EventBase ev);
  }

  public class ShowMessage : ActionBase
  {
    public override ActionType ActionType => ActionType.ShowMessage;

    public override void Do(Events.EventBase ev)
    {
      EClass.ui.Say($"Event received: {ev} at phase {ev.Phase}");
    }
  }
}
