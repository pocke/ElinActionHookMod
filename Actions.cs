using System;
using System.Collections.Generic;

namespace ActionHook;

public static class Actions
{
  public static readonly Dictionary<ActionType, Type> EventTypeToClass = new()
  {
      { ActionType.ShowMessage, typeof(ShowMessage) },
      { ActionType.ChangeEquipment, typeof(ChangeEquipment) },
      { ActionType.ChangeToolbelt, typeof(ChangeToolbelt) },
  };

  public enum ActionType
  {
    ShowMessage,
    ChangeEquipment,
    ChangeToolbelt,
  }

  public abstract class ActionBase
  {
    public abstract ActionType ActionType { get; }
    public string[] ActionArgs { get; set; }
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

  public class ChangeEquipment : ActionBase
  {
    public override ActionType ActionType => ActionType.ChangeEquipment;

    public override void Do(Events.EventBase ev)
    {
      var i = HotBarIndex();
      if (i < 0)
      {
        return;
      }

      WidgetHotbar.HotbarExtra.TryUse(i);
    }

    int HotBarIndex()
    {
      if (int.TryParse(ActionArgs[0], out var id))
      {
        return id - 1;
      }
      else
      {
        ActionHook.Log($"Invalid equipment ID: {ActionArgs[0]}");
        return -1;
      }
    }
  }

  public class ChangeToolbelt : ActionBase
  {
    public override ActionType ActionType => ActionType.ChangeToolbelt;

    public override void Do(Events.EventBase ev)
    {
      var widget = WidgetCurrentTool.Instance;
      widget.Select(-1); // Clear selection

      if (page < 0 || slot < 0)
      {
        ActionHook.Log($"Invalid toolbelt page or slot: {ActionArgs[0]}, {ActionArgs[1]}");
        return;
      }

      if (widget.page != page)
      {
        widget.SwitchPage();
      }

      widget.Select(slot, true);
    }

    int page => int.TryParse(ActionArgs[0], out var p) ? p - 1 : -1;
    int slot => int.TryParse(ActionArgs[1], out var s) ? s - 1 : -1;
  }
}
