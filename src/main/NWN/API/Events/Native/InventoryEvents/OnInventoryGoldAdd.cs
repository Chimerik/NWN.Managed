using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryGoldAdd : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldAdd { get; set; }

    NwObject IEvent.Context
    {
      get => null;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.AddGoldHook>
    {
      internal delegate void AddGoldHook(void* pCreature, int nGold, int bDisplayFeedback);

      protected override FunctionHook<AddGoldHook> RequestHook()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnAddGold;
        return HookService.RequestHook<AddGoldHook>(pHook, FunctionsLinux._ZN12CNWSCreature7AddGoldEii, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnAddGold(void* pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnInventoryGoldAdd eventData = ProcessEvent(new OnInventoryGoldAdd
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Gold = nGold,
        });

        if (!eventData.PreventGoldAdd)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }
      }
    }
  }
}