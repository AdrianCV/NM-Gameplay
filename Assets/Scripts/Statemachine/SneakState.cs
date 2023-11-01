using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class SneakState : PlayerBaseState
{
    public SneakState(PlayerStateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory) { }



    public override void EnterState()
    {
        _ctx.RunMultiplier /= 2;
        _ctx.WalkMultiplier /= 2;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        Move();
    }

    public override void ExitState()
    {
        _ctx.RunMultiplier *= 2;
        _ctx.WalkMultiplier *= 2;
    }

    public override void CheckSwitchState()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (_ctx.IsMovementPressed)
            {
                SwitchState(_factory.Moving());
            }
            else
            {
                SwitchState(_factory.Idle());
            }
        }
    }

    void Move()
    {
        _ctx.Rb.MovePosition(_ctx.Transform.position + _ctx.Transform.forward * _ctx.PlayerInput.normalized.magnitude * (_ctx.IsRunPressed ? _ctx.RunMultiplier : _ctx.WalkMultiplier) * Time.deltaTime);
        // _ctx.NotifyObservers(Notif.Sound, _ctx.transform);
    }

    public override void CollisionEnter(PlayerStateMachine manager, Collision collision)
    {
    }
}
