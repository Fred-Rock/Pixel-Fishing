using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected FishingSystem FishingSystem;

    public State(FishingSystem fishingSystem)
    {
        FishingSystem = fishingSystem;
    }

    public virtual IEnumerator BeginState()
    {
        yield break;
    }

    public virtual void UpdateState()
    {
    }

    public virtual void PlayerCastState()
    {
    }

    public virtual void PlayerReelState()
    {
    }
}