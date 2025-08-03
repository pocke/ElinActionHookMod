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
      { ActionType.Save, typeof(Save) },
  };

  public enum ActionType
  {
    ShowMessage,
    ChangeEquipment,
    ChangeToolbelt,
    Save,
  }

  public abstract class ActionBase
  {
    public abstract ActionType ActionType { get; }
    public string[] ActionArgs { get; set; }
    public abstract void Do(Events.EventBase ev);

    public virtual void Validate() {}
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
      WidgetHotbar.HotbarExtra.TryUse(i);
    }

    public override void Validate()
    {
      if (HotBarIndex() < 0)
      {
        throw new ValidationException($"Invalid hotbar index of ChangeEquipment action: {ActionArgs[0]}");
      }
    }

    int HotBarIndex()
    {
      if (int.TryParse(ActionArgs[0], out var id))
      {
        return id - 1;
      }
      else
      {
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

      if (widget.page != page)
      {
        widget.SwitchPage();
      }

      widget.Select(slot, true);
    }

    public override void Validate()
    {
      if (ActionArgs.Length < 2)
      {
        throw new ValidationException("ChangeToolbelt action requires two arguments: page and slot.");
      }
      if (page < 0 || 1 < page)
      {
        throw new ValidationException($"Invalid toolbelt page: {ActionArgs[0]}\n" +
                                      "Valid values are 1 or 2.");
      }
      if (slot < 0 || 8 < slot)
      {
        throw new ValidationException($"Invalid toolbelt slot: {ActionArgs[1]}\n" +
                                      "Valid values are 1 to 9.");
      }
    }

    int page => int.TryParse(ActionArgs[0], out var p) ? p - 1 : -1;
    int slot => int.TryParse(ActionArgs[1], out var s) ? s - 1 : -1;
  }

  public class Save : ActionBase
  {
    public override ActionType ActionType => ActionType.Save;

    public override void Do(Events.EventBase ev)
    {
      EClass.game.Save();
    }
  }
}
