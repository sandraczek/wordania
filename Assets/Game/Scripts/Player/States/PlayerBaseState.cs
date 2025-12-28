using UnityEngine;
public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx; // Kontekst (dostęp do gracza)
    protected PlayerStateFactory _factory; // Dostęp do innych stanów

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState(); // Do fizyki
    public abstract void ExitState();
    public abstract void CheckSwitchStates(); // Tutaj decydujemy o zmianie
}