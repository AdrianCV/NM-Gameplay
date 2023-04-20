using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory) { }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if (_ctx.Grounded && !Input.GetButton("Jump"))
        {
            _ctx.RB.velocity = Vector3.zero;
        }
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

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