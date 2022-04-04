using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private float _setupTime;
    // TODO: Add in starting position
    private Transform _searchArea;
    private float _searchMoveIncrement;
    private float _searchMoveSpeed;
    private float _waterBoundsLeft, _waterBoundsRight, _waterBoundsTop, _waterBoundsBottom;
    private PlayerInput _playerInput;
    private bool _canFish;

    public MoveState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _setupTime = FishingSystem.SetupTime;
        _searchArea = FishingSystem.Fisher.Hook.SearchArea;
        _searchMoveSpeed = FishingSystem.SearchMoveSpeed;
        _searchMoveIncrement = FishingSystem.SearchMoveIncrement;
        _playerInput = FishingSystem.PlayerInput;

        _waterBoundsLeft = FishingSystem.WaterBoundsLeft;
        _waterBoundsRight = FishingSystem.WaterBoundsRight;
        _waterBoundsTop = FishingSystem.WaterBoundsTop;
        _waterBoundsBottom = FishingSystem.WaterBoundsBottom;

        _canFish = false;
        
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Idle);
        FishingSystem.Fisher.Hook.ActivateHook(false);
        FishingSystem.StartCoroutine(ReadyCoroutine(_setupTime));

        EventHandler.CallHUDActiveEvent(true);
        EventHandler.CallCameraStateEvent(CameraManager.CameraState.Search);
        yield break;
    }

    public override void UpdateState()
    {
        if (_canFish != false)
        {
            MoveSearchArea();

            ConfineSearchArea();
        }
    }

    public override void PlayerCastState()
    {
        AttemptCast();
    }

    public override void PlayerReelState()
    {
        base.PlayerReelState();
    }

    private IEnumerator ReadyCoroutine(float countdown)
    {
        float countdownFrom = countdown;
        while (countdownFrom > 0)
        {
            countdownFrom = countdownFrom - Time.deltaTime;
            yield return null;
        }
        _canFish = true;
    }

    private void MoveSearchArea()
    {
        Vector3 newPos = new Vector3(_searchArea.position.x, _searchArea.position.y, _searchArea.position.z);

        if (_playerInput.MoveUp)
        {
            newPos = new Vector3(_searchArea.position.x, _searchArea.position.y + _searchMoveIncrement);
        }
        if (_playerInput.MoveLeft)
        {
            newPos = new Vector3(_searchArea.position.x - _searchMoveIncrement, _searchArea.position.y);
        }
        if (_playerInput.MoveDown)
        {
            newPos = new Vector3(_searchArea.position.x, _searchArea.position.y - _searchMoveIncrement);
        }
        if (_playerInput.MoveRight)
        {
            newPos = new Vector3(_searchArea.position.x + _searchMoveIncrement, _searchArea.position.y);
        }

        _searchArea.transform.position = Vector3.Lerp(_searchArea.position, newPos, _searchMoveSpeed * Time.deltaTime);
    }

    private void ConfineSearchArea()
    {
        Vector3 currentPos = _searchArea.transform.position;

        if (_searchArea.position.x < _waterBoundsLeft)
        {
            _searchArea.position = new Vector3(_waterBoundsLeft, currentPos.y);
        }
        if (_searchArea.position.x > _waterBoundsRight)
        {
            _searchArea.position = new Vector3(_waterBoundsRight, currentPos.y);
        }
        if (_searchArea.position.y < _waterBoundsBottom)
        {
            _searchArea.position = new Vector3(currentPos.x, _waterBoundsBottom);
        }
        if (_searchArea.position.y > _waterBoundsTop)
        {
            _searchArea.position = new Vector3(currentPos.x, _waterBoundsTop);
        }
    }

    public void AttemptCast()
    {
        if (_canFish != false)
        {
            _canFish = false;
            FishingSystem.CurrentState = FishingSystem.CastState;
            FishingSystem.SetState(FishingSystem.CurrentState);
        }
    }
}