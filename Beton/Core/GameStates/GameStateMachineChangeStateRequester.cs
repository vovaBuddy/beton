#nullable enable
using System;

namespace Beton.Core.GameStates
{
    public class GameStateMachineChangeStateRequester
    {
        public Type? RequestedState => _requestedState;
        public bool IsRequested => _requestedState != null;
        
        private Type? _requestedState;
        
        public void RequestChangeState<TState>() where TState : GameState
        {
            _requestedState = typeof(TState);
        }

        public void SetCompleted()
        {
            _requestedState = null;
        }
    }
}