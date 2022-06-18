namespace Common.StateMachine.States
{
    public class Empty : State
    {
        public Empty(StateMachine stateMachine) : base(nameof(Empty), stateMachine, Core.Empty.Dictionary<string, State>())
        {
        }
    }
}