using System;

namespace ActionHook;

public abstract class EventBase : IEquatable<EventBase>
{
  public abstract string EventType { get; }
  public virtual Events.Phase Phase { get; set; }

  public bool Equals(EventBase other) => EventType == other.EventType && Phase == other.Phase;
  public override bool Equals(object obj) => obj is EventBase other && Equals(other);
  public override int GetHashCode() => EventType.GetHashCode() ^ Phase.GetHashCode();
}

public class Events
{
  public enum Phase
  {
    Before,
    After
  }

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
      return base.Equals(obj) && obj is EnterZone other && ZoneType == other.ZoneType;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ ZoneType.GetHashCode();
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
      return base.Equals(obj) && obj is EventStartCrafting other && Skill == other.Skill;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ Skill.GetHashCode();
    }
  }
}
