using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSystem : StateMachineBase
{
    #region Fields
    private State _currentState;
    [HideInInspector] public OpeningState OpeningState;
    [HideInInspector] public MoveState MoveState;
    [HideInInspector] public CastState CastState;
    [HideInInspector] public WaitState WaitState;
    [HideInInspector] public HookState HookState;
    [HideInInspector] public ReelState ReelState;
    [HideInInspector] public CatchState CatchState;
    [HideInInspector] public CooldownState CooldownState;

    [Header("Input")]
    [SerializeField] PlayerInput _playerInput;

    [Header("Setup")]
    [SerializeField] private float _setupTime = 5f;
    [SerializeField] private Vector3 _startingPosition = new Vector3(0f, 0f, 0f);

    [Header("Movement")]
    [SerializeField] private float _waterBoundsLeft = -7f;
    [SerializeField] private float _waterBoundsRight = 22f;
    [SerializeField] private float _waterBoundsTop = -3f;
    [SerializeField] private float _waterBoundsBottom = -4.5f;
    [SerializeField] private float _searchMoveIncrement = 1f;
    [SerializeField] private float _searchMoveSpeed = 1f;

    [Header("Casting")]
    [SerializeField] private Fisher _fisher;
    [SerializeField] private float _castTime = 2f;
    [SerializeField] private float _castCooldown = 3f;
    [SerializeField] private float _boatTravelDuration = 1f;

    [Header("Hooking")]
    [SerializeField] private float _hookDelay = 5f;
    #endregion

    #region Properties
    public State CurrentState { get { return _currentState; } set { _currentState = value; } }
    public float SetupTime { get { return _setupTime; } private set { } }
    public PlayerInput PlayerInput { get { return _playerInput; } set { } }
    public float SearchMoveIncrement { get { return _searchMoveIncrement; } private set { } }
    public float SearchMoveSpeed { get { return _searchMoveSpeed; } private set { } }
    public float WaterBoundsLeft { get { return _waterBoundsLeft; } private set { } }
    public float WaterBoundsRight { get { return _waterBoundsRight; } private set { } }
    public float WaterBoundsTop { get { return _waterBoundsTop; } private set { } }
    public float WaterBoundsBottom { get { return _waterBoundsBottom; } private set { } }
    public float BoatTravelDuration { get { return _boatTravelDuration; } private set { } }
    public float HookDelay { get { return _hookDelay; } private set { } }
    public Fisher Fisher { get { return _fisher; } private set { } }
    public float CastTime { get { return _castTime; } private set { } }
    public float CastCooldown { get { return _castCooldown; } private set { } }
    #endregion

    public void OnEnable()
    {
        EventHandler.StartGameEvent += StartStateMachine;
        EventHandler.PlayerCastAttemptEvent += PlayerAttemptCast;
        EventHandler.PlayerReelAttemptEvent += PlayerAttemptReel;
    }

    public void OnDisable()
    {
        EventHandler.StartGameEvent -= StartStateMachine;
        EventHandler.PlayerCastAttemptEvent -= PlayerAttemptCast;
        EventHandler.PlayerReelAttemptEvent -= PlayerAttemptReel;
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public void PlayerAttemptCast()
    {
        if (_currentState != null)
        {
            _currentState.PlayerCastState();
        }
    }

    public void PlayerAttemptReel()
    {
        if (_currentState != null)
        {
            _currentState.PlayerReelState();
        }
    }

    private void Init()
    {
        transform.position = _startingPosition;
    }

    private void InitializeStates()
    {
        OpeningState = new OpeningState(this);
        MoveState = new MoveState(this);
        WaitState = new WaitState(this);
        CastState = new CastState(this);
        HookState = new HookState(this);
        ReelState = new ReelState(this);
        CatchState = new CatchState(this);
        CooldownState = new CooldownState(this);
    }

    private void StartStateMachine()
    {
        Init();

        InitializeStates();

        _currentState = OpeningState;
        SetState(_currentState);
    }
}