using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : State
{
    private Fish _hookedFish;
    private bool _isFishHooked;
    public HookState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        Debug.Log("HookState entered...");
        _isFishHooked = false;
        yield return null;
    }

    public override void UpdateState()
    {
        // Needs to run in Update to monitor hooked fish
        HookFish();
    }

    public override void PlayerReelState()
    {
        ResetHook();
    }

    private void HookFish()
    {
        _hookedFish = FishingSystem.Fisher.Hook.HookedFish;

        if (_hookedFish != null && !_isFishHooked)
        {
            _isFishHooked = true;
            EventHandler.CallFishHookedEvent();
            FishingSystem.CurrentState = FishingSystem.ReelState;
            FishingSystem.SetState(FishingSystem.CurrentState);
        }
    }

    private void ResetHook()
    {
        if (_hookedFish == null)
        {
            FishingSystem.CurrentState = FishingSystem.MoveState;
            FishingSystem.SetState(FishingSystem.CurrentState);
        }
    }
}