using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningState : State
{
    private Cutscene _cutscene;
    private bool _isPlaying;

    public OpeningState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _cutscene = FishingSystem.FindObjectOfType<Cutscene>();
        _cutscene.PlayOpeningCutscene();
        FishingSystem.Fisher.SetFisherState(Fisher.FisherState.Inactive);
        EventHandler.CallCameraStateEvent(CameraManager.CameraState.Cutscene);
        yield return null;
        FishingSystem.StartCoroutine(ChangeState());
    }

    public override void UpdateState()
    {
        _isPlaying = _cutscene.IsCutscenePlaying();
    }

    private IEnumerator ChangeState()
    {
        yield return new WaitUntil(() => !_isPlaying);
        FishingSystem.CurrentState = FishingSystem.MoveState;
        FishingSystem.SetState(FishingSystem.CurrentState);
    }
}
