using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchState : State
{
    private Fish _caughtFish;
    private float _cooldown;
    public CatchState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _caughtFish = FishingSystem.Fisher.Hook.HookedFish;
        _cooldown = FishingSystem.CastCooldown;
        yield return null;

        EventHandler.CallHUDActiveEvent(false);
        EventHandler.CallFishCaughtEvent(_caughtFish);
        FishingSystem.StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _caughtFish.gameObject.SetActive(false);
        FishingSystem.CurrentState = FishingSystem.MoveState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }
}
