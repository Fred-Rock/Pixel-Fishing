using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownState : State
{
    private float _timer = 0;

    public CooldownState(FishingSystem fishingSystem) : base(fishingSystem)
    {
    }

    public override IEnumerator BeginState()
    {
        _timer = 0;

        Debug.Log("Waiting to cast again...");
        //FishingSystem.FishOnHook = new FishData();
        //FishingSystem.SearchList.Clear();

        yield break;
    }

    public override void UpdateState()
    {
        if (_timer <= FishingSystem.CastCooldown)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            FishingSystem.SetState(FishingSystem.CastState);
        }
    }
}
