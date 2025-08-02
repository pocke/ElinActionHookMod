using System;

namespace ActionHook;

public abstract class EventBase : IEquatable<EventBase>
{
  public abstract string EventType { get; }
  public bool Equals(EventBase other) => EventType == other.EventType;
  public override bool Equals(object obj) => obj is EventBase other && Equals(other);
  public override int GetHashCode() => EventType.GetHashCode();
}

public class Events
{
  public enum ZoneType
  {
    Nefia
  }

  public enum Skill
  {
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

  public class EnterZone : EventBase
  {
    public override string EventType => "EnterZone";
    public ZoneType ZoneType { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is EnterZone other)
      {
        return ZoneType == other.ZoneType;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return EventType.GetHashCode() ^ ZoneType.GetHashCode();
    }
  }

  public class EventSleep : EventBase
  {
    public override string EventType => "EventSleep";
  }

  public class EventGoDownStairs : EventBase
  {
    public override string EventType => "EventGoDownStairs";
  }

  public class EventGoUpStairs : EventBase
  {
    public override string EventType => "EventGoUpStairs";
  }

  public class EventStartCrafting : EventBase
  {
    public override string EventType => "EventStartCrafting";
    public Skill Skill { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is EventStartCrafting other)
      {
        return Skill == other.Skill;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return EventType.GetHashCode() ^ Skill.GetHashCode();
    }
  }
}
