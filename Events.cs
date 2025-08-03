using System;
using System.Collections.Generic;

namespace ActionHook;

public class Events
{

  public static readonly Dictionary<EventType, Type> EventTypeToClass = new()
  {
      { EventType.EnterZone, typeof(EnterZone) },
      { EventType.Sleep, typeof(Sleep) },
      { EventType.GoDownStairs, typeof(GoDownStairs) },
      { EventType.GoUpStairs, typeof(GoUpStairs) },
      { EventType.StartCrafting, typeof(StartCrafting) },
  };

  public enum EventType
  {
    EnterZone,
    Sleep,
    GoDownStairs,
    GoUpStairs,
    StartCrafting,
  }

  public enum Phase
  {
    Before,
    After
  }

  public enum SubType
  {
    // For EnterZone
    Nefia,
    PCFaction,

    // For StartCrafting
    Carpentry,
    Blacksmith,
    Alchemy,
    Sculpture,
    Jewelry,
    Weaving,
    Handicraft,
    Cooking,
    Reading,
  }

  public abstract class EventBase : IEquatable<EventBase>
  {
    public abstract EventType EventType { get; }
    public SubType? SubType { get; set; }
    public virtual Phase Phase { get; set; }

    public abstract void Validate();

    public bool Equals(EventBase other) => EventType == other.EventType && SubType == other.SubType && Phase == other.Phase;
    public override bool Equals(object obj) => obj is EventBase other && Equals(other);
    public override int GetHashCode() => EventType.GetHashCode() ^ SubType.GetHashCode() ^ Phase.GetHashCode();
  }

  public class EnterZone : EventBase
  {
    public override EventType EventType => EventType.EnterZone;

    public override void Validate()
    {
      if (SubType != Events.SubType.Nefia && SubType != Events.SubType.PCFaction)
      {
        throw new ValidationException($"Invalid SubType for EnterZone event: {SubType}");
      }
    }
  }

  public class Sleep : EventBase
  {
    public override EventType EventType => EventType.Sleep;

    public override void Validate()
    {
      if (SubType != null)
      {
        throw new ValidationException($"SubType must not be set for Sleep event, but got: {SubType}");
      }
    }
  }

  public class GoDownStairs : EventBase
  {
    public override EventType EventType => EventType.GoDownStairs;

    public override void Validate()
    {
      if (SubType != null)
      {
        throw new ValidationException($"SubType must not be set for Sleep event, but got: {SubType}");
      }
    }
  }

  public class GoUpStairs : EventBase
  {
    public override EventType EventType => EventType.GoUpStairs;

    public override void Validate()
    {
      if (SubType != null)
      {
        throw new ValidationException($"SubType must not be set for Sleep event, but got: {SubType}");
      }
    }
  }

  public class StartCrafting : EventBase
  {
    public override EventType EventType => EventType.StartCrafting;

    public override void Validate()
    {
      if (SubType == Events.SubType.Nefia || SubType == Events.SubType.PCFaction)
      {
        throw new ValidationException($"Invalid SubType for StartCrafting event: {SubType}");
      }
    }
  }
}
