using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CastState : State
{
    private Transform _searchArea;
    private Transform _boat;
    private Transform _linePosition;
    private float _speed;
    private float _castTime;

    public CastState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _searchArea = FishingSystem.Fisher.Hook.SearchArea;
        _boat = FishingSystem.Fisher.Boat;
        _linePosition = FishingSystem.Fisher.LinePosition;
        _speed = FishingSystem.BoatTravelDuration;
        _castTime = FishingSystem.CastTime;

        FishingSystem.StartCoroutine(MoveToCastPosition());

        EventHandler.CallCameraStateEvent(CameraManager.CameraState.Boat);
        yield return null;
    }

    private IEnumerator MoveToCastPosition()
    {
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Rowing);

        float distBetweenBoatCenterAndLine = _boat.position.x - _linePosition.position.x;
        float hookXPos = _searchArea.position.x;
        float destinationXPos = hookXPos + distBetweenBoatCenterAndLine;

        //Tween moveToPosition = _boat.DOMoveX(destinationXPos, _speed);
        float moveDuration = Mathf.Abs(destinationXPos - _boat.position.x);
        Tween moveToPosition = _boat.DOMoveX(destinationXPos, moveDuration);
        yield return moveToPosition.WaitForCompletion();
        FishingSystem.StartCoroutine(CastLine());
    }

    private IEnumerator CastLine()
    {
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Idle);
        yield return new WaitForSeconds(_castTime);

        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Fishing);
        FishingSystem.Fisher.Hook.ActivateHook(true);
        EventHandler.CallCastEvent();
        
        FishingSystem.CurrentState = FishingSystem.WaitState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }
}