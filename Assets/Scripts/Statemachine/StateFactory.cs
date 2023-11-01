using StateMachine;

public class StateFactory
{
    PlayerStateMachine _context;

    public StateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new IdleState(_context, this);
    }
    public PlayerBaseState Moving()
    {
        return new MovingState(_context, this);
    }

    internal PlayerBaseState Sneak()
    {
        return new SneakState(_context, this);
    }
}
