namespace ActionHook;

public abstract class EventBase
{
  public abstract string EventType { get; }
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
  }
}
