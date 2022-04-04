using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Transform _actor;
    [SerializeField] private Vector3 _actorStartingPos;
    [SerializeField] private float _actorEndPosX;
    [SerializeField] private Animator _actorAnim;
    [SerializeField] private float _actorWaitTime = 1f;
    [SerializeField] private float _actorMoveToPositionDuration = 1.5f;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1;
    private bool _isCutscenePlaying = false;

    private void Start()
    {
        DOTween.Init();
        _actor.gameObject.SetActive(false);
        _isCutscenePlaying = false;
    }

    public void PlayOpeningCutscene()
    {
        _actor.localPosition = _actorStartingPos;
        _isCutscenePlaying = true;
        StartCoroutine(StartActorAtPosition());
    }

    public bool IsCutscenePlaying()
    {
        if (_isCutscenePlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Opening cutscene coroutines
    private IEnumerator StartActorAtPosition()
    {
        _actor.gameObject.SetActive(true);
        yield return new WaitForSeconds(_actorWaitTime);
        StartCoroutine(MoveActorToPosition());
    }

    private IEnumerator MoveActorToPosition()
    {
        _actorAnim.SetBool("isWalking", true);
        Tween moveActor = _actor.DOMoveX(_actorEndPosX, _actorMoveToPositionDuration).SetEase(Ease.Linear);
        yield return moveActor.WaitForCompletion();
        StartCoroutine(EndActorAtPosition());
    }

    private IEnumerator EndActorAtPosition()
    {
        _actorAnim.SetBool("isWalking", false);
        yield return new WaitForSeconds(_actorWaitTime);
        StartCoroutine(DeactivateActor());
    }

    private IEnumerator DeactivateActor()
    {
        StartCoroutine(EndCutscene());
        yield return new WaitForSeconds(_fadeDuration);
        _actor.gameObject.SetActive(false);
    }

    private IEnumerator EndCutscene()
    {
        StartCoroutine(SceneFadeCoroutine(_fadeDuration));
        yield return new WaitForSeconds(_fadeDuration);
        _isCutscenePlaying = false;
    }
    #endregion

    private IEnumerator SceneFadeCoroutine(float fadeDuration)
    {
        Tween fade = _fadeImage.DOFade(1, fadeDuration);
        yield return fade.WaitForCompletion();
        _fadeImage.DOFade(0, fadeDuration);
    }
}