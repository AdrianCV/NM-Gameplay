using StateMachine;
using UnityEngine;

public class MovingState : PlayerBaseState
{
    static readonly int IsRunning = Animator.StringToHash("IsRunning");
    public MovingState(PlayerStateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory) { }
    public override void EnterState()
    {
        _ctx.Animator.SetBool(IsRunning, true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        Move();
    }

    void OldMovement()
    {
        if (_ctx.IsRunPressed)
        {
            _ctx.CharacterController.Move(_ctx.CurrentRunMovement * Time.deltaTime);
        }
        else
        {
            _ctx.CharacterController.Move(_ctx.CurrentMovement * Time.deltaTime);
        }
    }

    void Move()
    {
        _ctx.Rb.MovePosition(_ctx.Transform.position + _ctx.Transform.forward * _ctx.PlayerInput.normalized.magnitude * (_ctx.IsRunPressed ? _ctx.RunMultiplier : _ctx.WalkMultiplier) * Time.deltaTime);
        // _ctx.NotifyObservers(Notif.Sound, _ctx.transform);
    }

    public override void ExitState()
    {
        _ctx.Animator.SetBool(IsRunning, false);
        _ctx.Rb.angularVelocity = Vector3.zero;
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Idle());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchState(_factory.Sneak());
        }
    }

    public override void CollisionEnter(PlayerStateMachine manager, Collision collision)
    {

    }
}
