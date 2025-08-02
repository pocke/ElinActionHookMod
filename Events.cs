using System;
using System.Collections.Generic;

namespace ActionHook;

public class Events
{

  public static readonly Dictionary<EventType, Type> EventTypeToClass = new()
  {
      { EventType.EnterZone, typeof(EnterZone) },
      { EventType.Sleep, typeof(EventSleep) },
      { EventType.GoDownStairs, typeof(EventGoDownStairs) },
      { EventType.GoUpStairs, typeof(EventGoUpStairs) },
      { EventType.StartCrafting, typeof(EventStartCrafting) },
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
    public abstract Events.EventType EventType { get; }
    public virtual Events.SubType? SubType { get; set; }
    public virtual Events.Phase Phase { get; set; }

    public bool Equals(EventBase other) => EventType == other.EventType && SubType == other.SubType && Phase == other.Phase;
    public override bool Equals(object obj) => obj is EventBase other && Equals(other);
    public override int GetHashCode() => EventType.GetHashCode() ^ SubType.GetHashCode() ^ Phase.GetHashCode();
  }

  public class EnterZone : EventBase
  {
    public override EventType EventType => EventType.EnterZone;
  }

  public class EventSleep : EventBase
  {
    public override EventType EventType => EventType.Sleep;
  }

  public class EventGoDownStairs : EventBase
  {
    public override EventType EventType => EventType.GoDownStairs;
  }

  public class EventGoUpStairs : EventBase
  {
    public override EventType EventType => EventType.GoUpStairs;
  }

  public class EventStartCrafting : EventBase
  {
    public override EventType EventType => EventType.StartCrafting;
  }
}
