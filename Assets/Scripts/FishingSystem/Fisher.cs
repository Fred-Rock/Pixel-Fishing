using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    #region Fields
    [SerializeField] private Hook _hook;
    [SerializeField] private Transform _boat;
    [SerializeField] private Transform _searchArea;
    [SerializeField] private Transform _linePosition;
    [SerializeField] private Transform _idleAndFishingSprite;
    [SerializeField] private Transform _rowingSprite;
    [SerializeField] private Animator _fishingAnim;
    private FisherState _fisherState;
    #endregion

    #region Fisher States
    public enum FisherState
    {
        Inactive,
        Idle,
        Rowing,
        Fishing,
        Catching
    }
    #endregion

    #region Properties
    public Transform Boat { get { return _boat; } private set { } }
    public Hook Hook { get { return _hook; } private set { } }
    public Transform LinePosition { get { return _linePosition; } private set { } }
    public FisherState State { get { return _fisherState; } set { } }
    #endregion

    private void Start()
    {
        _fisherState = FisherState.Inactive;
    }

    private void Update()
    {
        switch (_fisherState)
        {
            case FisherState.Inactive:
                _searchArea.gameObject.SetActive(false);
                _idleAndFishingSprite.gameObject.SetActive(false);
                break;
            case FisherState.Idle:
                _searchArea.gameObject.SetActive(true);
                _rowingSprite.gameObject.SetActive(false);
                _idleAndFishingSprite.gameObject.SetActive(true);
                _fishingAnim.SetBool("isIdle", true);
                _fishingAnim.SetBool("isFishing", false);
                break;
            case FisherState.Rowing:
                _rowingSprite.gameObject.SetActive(true);
                _idleAndFishingSprite.gameObject.SetActive(false);
                break;
            case FisherState.Fishing:
                _fishingAnim.SetBool("isIdle", false);
                _fishingAnim.SetBool("isFishing", true);
                break;
            case FisherState.Catching:
                _fishingAnim.SetTrigger("fishCaught");
                _fishingAnim.SetBool("isIdle", true);
                break;
            default:
                break;
        }
    }

    public void SetFisherState(FisherState state)
    {
        _fisherState = state;
    }
}
