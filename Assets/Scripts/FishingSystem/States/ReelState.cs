using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelState : State
{
    private Fish _hookedFish;
    private float _timeToReelFish;
    private bool _canReelFish;
    private bool _caughtInTime;
    public ReelState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _hookedFish = FishingSystem.Fisher.Hook.HookedFish;
        _timeToReelFish = _hookedFish.ReelTime;
        _canReelFish = true;
        _caughtInTime = false;
        
        EventHandler.CallReelEvent(_timeToReelFish);

        FishingSystem.StartCoroutine(CountdownToCatchFish());
        FishingSystem.StartCoroutine(CountdownToLoseFish());
        yield return null;
    }

    public override void PlayerCastState()
    {
        base.PlayerCastState();
    }

    public override void PlayerReelState()
    {
        //ReelFish();

        AttemptCatch();
    }

    private void ReelFish()
    {
        Debug.Log(_hookedFish);
        if (_canReelFish)
        {
            Debug.Log($"You snagged it!");
            FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Catching);
            
            EventHandler.CallCameraStateEvent(CameraManager.CameraState.Catch);

            FishingSystem.CurrentState = FishingSystem.CatchState;
            FishingSystem.SetState(FishingSystem.CurrentState);
        }
        else if (!_canReelFish)
        {
            if (_hookedFish != null)
            {
                Debug.Log("Fish got away. Time to cast again.");
                EventHandler.CallFishLostEvent();
            }
            else
            {
                FishingSystem.Fisher.Hook.ActivateHook(false);
            }
            FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Idle);
            FishingSystem.CurrentState = FishingSystem.MoveState;
            FishingSystem.SetState(FishingSystem.CurrentState);
        }
    }

    private void AttemptCatch()
    {
        if (!_caughtInTime)
        {
            _caughtInTime = true;
            EventHandler.CallSuccessEvent();
        }
    }

    private IEnumerator CountdownToLoseFish()
    {
        yield return new WaitForSeconds(_timeToReelFish);
        _canReelFish = false;
    }

    private IEnumerator CountdownToCatchFish()
    {
        yield return new WaitForSeconds(_timeToReelFish);
        if (_caughtInTime)
        {
            CatchFish();
        }
        else
        {
            LoseFish();
        }
    }

    private void CatchFish()
    {
        Debug.Log($"You snagged it!");
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Catching);

        EventHandler.CallCameraStateEvent(CameraManager.CameraState.Catch);

        FishingSystem.CurrentState = FishingSystem.CatchState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }

    private void LoseFish()
    {
        if (_hookedFish != null)
        {
            Debug.Log("Fish got away. Time to cast again.");
            EventHandler.CallFishLostEvent();
        }
        else
        {
            FishingSystem.Fisher.Hook.ActivateHook(false);
        }
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Idle);
        FishingSystem.CurrentState = FishingSystem.MoveState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }
}