using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private PlayerController _controller;

    private enum State { IDLE, WALK, RUN, JUMP, ROLL, WALLSLIDE }

    private State state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        state = State.IDLE;
    }

    private void Update()
    {
        state = CheckState();
        UpdateState(state);
        //Debug.Log(state);
    }

    private void UpdateState(State currentState)
    {
        switch(currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.WALK:
                Walk();
                break;
            case State.JUMP:
                Jump();
                break;
            case State.WALLSLIDE:
                WallSlide();
                break;
            default:
                Idle();
                break;
        }
    }

    private State CheckState()
    {
        anim.SetBool("isGrounded", _controller.getOnGround());
        anim.SetBool("isWallSliding", _controller.getWallSlide());

        if (_controller.getIsMove() == true)
        {
            state = State.WALK;
        }
        else
            state = State.IDLE;

        if (_controller.getOnGround() != true)
            state = State.JUMP;

        if (_controller.getOnGround() == false && _controller.getWallSlide() == true)
            state = State.WALLSLIDE;

        //Debug.Log(state);
        return state;
    }

    private void Idle()
    {
        if (_controller.getIsMove() == false)
            anim.Play("Idle");
        //Debug.Log(state);
    }

    private void Walk()
    {
        if(_controller.getIsMove() == true)
        {
            anim.Play("Walk");
          //  Debug.Log(state);
        }
    }

    private void Jump()
    {
        anim.SetBool("isGrounded", _controller.getOnGround());
        anim.SetFloat("yVelocity", _controller.getYVelocity());
    }

    private void WallSlide()
    {
        anim.SetBool("isWallSliding", _controller.getWallSlide());
    }
}
