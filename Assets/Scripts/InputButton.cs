using System;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    public static float VerticalInput;
    public static float HorizontalInput;

    public enum State
    {
        None,
        Down,
        Up
    }

    private State state = State.None;

    private void Update()
    {
        GetKeyboardInput();

        //if (state == State.None)
        //{
        //    VerticalInput = 0;
        //}
        //else if (state == State.Up)
        //{
        //    VerticalInput = 1;
        //}
        //else
        //{
        //    VerticalInput = -1;
        //}
    }

    private void GetKeyboardInput()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        VerticalInput = v;
        HorizontalInput= h;
    }

    public void OnMoveUpButtonPressed()
    {
        state = State.Up;
    }

    public void OnMoveUpButtonUp()
    {
        if(state == State.Up)
        {
            state = State.None;
        }
    }

    public void OnMoveDownButtonPressed()
    {
        state = State.Down;
    }

    public void OnMoveDownButtonUp()
    {
        if(state == State.Down)
        {
            state = State.None;
        }
    }
}