using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    #region Camera events
    public static event Action<CameraManager.CameraState> CameraStateEvent;
    public static void CallCameraStateEvent(CameraManager.CameraState cameraState)
    {
        if (CameraStateEvent != null)
        {
            CameraStateEvent(cameraState);
        }
    }
    #endregion

    #region Fishing system events
    public static event Action CastEvent;
    public static void CallCastEvent()
    {
        if (CastEvent != null)
        {
            CastEvent();
        }
    }

    public static event Action FishHookedEvent;
    public static void CallFishHookedEvent()
    {
        if (FishHookedEvent != null)
        {
            FishHookedEvent();
        }
    }

    public static event Action FishLostEvent;
    public static void CallFishLostEvent()
    {
        if (FishLostEvent != null)
        {
            FishLostEvent();
        }
    }

    public static event Action<float> ReelEvent;
    public static void CallReelEvent(float reelTime)
    {
        if (ReelEvent != null)
        {
            ReelEvent(reelTime);
        }
    }

    public static event Action<Fish> FishCaughtEvent;
    public static void CallFishCaughtEvent(Fish fish)
    {
        if (FishCaughtEvent != null)
        {
            FishCaughtEvent(fish);
        }
    }
    #endregion

    #region UI events
    public static event Action StartGameEvent;
    public static void CallStartGameEvent()
    {
        if (StartGameEvent != null)
        {
            StartGameEvent();
        }
    }

    public static event Action<bool> HUDActiveEvent;
    public static void CallHUDActiveEvent(bool isActive)
    {
        if (HUDActiveEvent != null)
        {
            HUDActiveEvent(isActive);
        }
    }

    public static event Action MenuButtonClickedEvent;
    public static void CallMenuButtonClickedEvent()
    {
        if (MenuButtonClickedEvent != null)
        {
            MenuButtonClickedEvent();
        }
    }

    public static event Action PlayerCastAttemptEvent;
    public static void CallPlayerCastAttemptEvent()
    {
        if (PlayerCastAttemptEvent != null)
        {
            PlayerCastAttemptEvent();
        }
    }

    public static event Action PlayerReelAttemptEvent;
    public static void CallPlayerReelAttemptEvent()
    {
        if (PlayerReelAttemptEvent != null)
        {
            PlayerReelAttemptEvent();
        }
    }
    #endregion

    #region Generic events
    public static event Action SuccessEvent;
    public static void CallSuccessEvent()
    {
        if (SuccessEvent != null)
        {
            SuccessEvent();
        }
    }

    public static event Action FailureEvent;
    public static void CallFailureEvent()
    {
        if (FailureEvent != null)
        {
            FailureEvent();
        }
    }
    #endregion
}