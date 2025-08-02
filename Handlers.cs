namespace ActionHook;

public abstract class HandlerBase
{
  public abstract string HandlerType { get; }
  public abstract void Handle(EventBase ev);
}

public class Handlers
{
  public class Say : HandlerBase
  {
    public override string HandlerType => "say";

    public override void Handle(EventBase ev) {
      Msg.SayRaw("Event received: " + ev.EventType);
    }
  }
}
