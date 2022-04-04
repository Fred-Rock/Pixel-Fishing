using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    private float _hookDelay;
    public WaitState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        Debug.Log("WaitState started...");
        ResetHookDelay();
        FishingSystem.StartCoroutine(WaitForHook());
        yield return null;
    }

    private IEnumerator WaitForHook()
    {
        yield return new WaitForSeconds(_hookDelay);
        FishingSystem.CurrentState = FishingSystem.HookState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }

    private void ResetHookDelay()
    {
        _hookDelay = FishingSystem.HookDelay;
    }
}
