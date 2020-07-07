using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NWN.API
{
  /// <summary>
  /// Awaiters for running NWN code in an async context.
  /// </summary>
  public static partial class NwTask
  {
    public static async Task Delay(TimeSpan delay)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      await RunAndAwait(() => delay < stopwatch.Elapsed);
    }

    public static async Task NextFrame() => await DelayFrame(1);

    public static async Task DelayFrame(int frames)
    {
      frames++;
      await RunAndAwait(() =>
      {
        frames--;
        return frames <= 0;
      });
    }

    public static async Task WaitUntil(Func<bool> test) => await RunAndAwait(test);

    public static async Task WaitUntilValueChanged<T>(Func<T> valueSource)
    {
      T currentVal = valueSource();
      await RunAndAwait(() => !valueSource().Equals(currentVal));
    }
  }
}