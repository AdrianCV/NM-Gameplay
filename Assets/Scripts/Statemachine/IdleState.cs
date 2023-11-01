using StateMachine;
using UnityEngine;

public class IdleState : PlayerBaseState
{
    static readonly int IsIdle = Animator.StringToHash("IsIdle");
    public IdleState(PlayerStateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(IsIdle, true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if (_ctx.Grounded && !Input.GetButton("Jump"))
        {
            _ctx.Rb.velocity = Vector3.zero;
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        _ctx.Animator.SetBool(IsIdle, false);
    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Moving());
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