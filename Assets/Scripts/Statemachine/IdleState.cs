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