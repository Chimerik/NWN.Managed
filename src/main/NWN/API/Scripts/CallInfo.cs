namespace NWN.API
{
  /// <summary>
  /// Meta information for script calls, consumed by ScriptHandler attributed methods in service classes.
  /// </summary>
  public class CallInfo
  {
    private static readonly ScriptParams scriptParams = new ScriptParams();

    /// <summary>
    /// Gets the parameters set for this script call.<br/>
    /// NOTE: variable values are NOT guaranteed outside of this script context, and must be read before any async method/lambda is invoked.
    /// </summary>
    public ScriptParams ScriptParams
    {
      get => scriptParams;
    }

    /// <summary>
    /// The object that is currently running on this script.
    /// </summary>
    public NwObject ObjectSelf { get; }

    public CallInfo(NwObject objSelf)
    {
      this.ObjectSelf = objSelf;
    }
  }
}