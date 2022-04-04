using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _xInput, _yInput;
    private float _castAndReelInput;
    [HideInInspector] public bool MoveRight, MoveLeft, MoveUp, MoveDown;

    public float XInput { get { return _xInput; } set { } }
    public float YInput { get { return _yInput; } set { } }
    public float CastAndReelInput { get { return _castAndReelInput; } set { } }

    private void Update()
    {
        GetMoveInput();
    }

    private void GetMoveInput()
    {
        _xInput = Input.GetAxis("Horizontal");
        _yInput = Input.GetAxis("Vertical");

        if (_xInput > 0)
        {
            MoveRight = true;
        }
        else if (_xInput < 0)
        {
            MoveLeft = true;
        }
        else if (_yInput > 0)
        {
            MoveUp = true;
        }
        else if (_yInput < 0)
        {
            MoveDown = true;
        }
        else
        {
            MoveRight = false;
            MoveLeft = false;
            MoveUp = false;
            MoveDown = false;
        }
    }
}
