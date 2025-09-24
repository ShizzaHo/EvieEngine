namespace EvieEngine.AI
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Tick();
    }
}