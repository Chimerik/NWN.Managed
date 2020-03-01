using NWN;

namespace NWM.API
{
  public class NwWaypoint : NwGameObject
  {
    protected internal NwWaypoint(uint objectId) : base(objectId) {}
    public static NwWaypoint Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwWaypoint>(ObjectType.Item, template, location, useAppearAnim, newTag);
    }
  }
}