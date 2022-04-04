using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioPlayAndStop;
    [SerializeField] private AudioSource _audioOneShot;
    [SerializeField] private AudioSource _audioAmbient;
    [SerializeField] private AudioSource _audioBackgroundMusic;
    [SerializeField] private AudioClip _buttonClickSFX;
    [SerializeField] private AudioClip _alertSFX;
    [SerializeField] private AudioClip _successSFX;
    [SerializeField] private AudioClip _failureSFX;
    [SerializeField] private AudioClip _walkSFX;
    [SerializeField] private AudioClip _splashSFX;
    [SerializeField] private AudioClip _castSFX;
    [SerializeField] private AudioClip _reelSFX;

    private void OnEnable()
    {
        EventHandler.MenuButtonClickedEvent += PlayButtonClickSFX;
        //EventHandler.FishHookedEvent += PlaySuccessSFX;
        EventHandler.CastEvent += PlayCastSFX;
        EventHandler.ReelEvent += PlayReelSFX;
        EventHandler.FishLostEvent += PlayFailureSFX;
        EventHandler.StartGameEvent += PlayBackgroundMusic;

        EventHandler.SuccessEvent += PlaySuccessSFX;
        EventHandler.FailureEvent += PlayFailureSFX;
    }

    private void OnDisable()
    {
        EventHandler.MenuButtonClickedEvent -= PlayButtonClickSFX;
        //EventHandler.FishHookedEvent -= PlaySuccessSFX;
        EventHandler.CastEvent -= PlayCastSFX;
        EventHandler.ReelEvent -= PlayReelSFX;
        EventHandler.FishLostEvent -= PlayFailureSFX;
        EventHandler.StartGameEvent -= PlayBackgroundMusic;

        EventHandler.SuccessEvent -= PlaySuccessSFX;
        EventHandler.FailureEvent -= PlayFailureSFX;
    }

    private void Update()
    {
        DebugSFX();
    }

    private void Start()
    {
        PlayAmbient();
    }

    private void PlayAmbient()
    {
        _audioAmbient.Play();
    }

    private void PlayBackgroundMusic()
    {
        _audioBackgroundMusic.Play();
    }

    private void PlayButtonClickSFX()
    {
        _audioOneShot.PlayOneShot(_buttonClickSFX);
    }

    private void PlaySuccessSFX()
    {
        _audioOneShot.PlayOneShot(_successSFX);
    }

    private void PlayFailureSFX()
    {
        _audioOneShot.PlayOneShot(_failureSFX);
    }

    #region Fishing SFX
    private void PlayCastSFX()
    {
        _audioOneShot.PlayOneShot(_castSFX);
    }

    private void PlaySplashSFX()
    {
        _audioOneShot.PlayOneShot(_splashSFX);
    }

    private void PlayWalkSFX(float playLength)
    {
        _audioPlayAndStop.clip = _walkSFX;
        _audioPlayAndStop.Play();
        Invoke("StopSFX", playLength);
    }

    private void PlayReelSFX(float playLength)
    {
        _audioPlayAndStop.clip = _reelSFX;
        _audioPlayAndStop.Play();
        Invoke("StopSFX", playLength);
    }
    #endregion

    private void StopSFX()
    {
        _audioPlayAndStop.Stop();
    }

    private void DebugSFX()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Calling DebugSFX");
            _audioPlayAndStop.PlayOneShot(_splashSFX);
        }
    }
}
