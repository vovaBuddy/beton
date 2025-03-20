#nullable enable
using System;
using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.GameStates
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, GameState> _states = new();

        private GameState _currentState = default!;
        
        private readonly GameStateMachineChangeStateRequester _changeStateRequester;
        
        private bool _isBusy = false;

        public GameStateMachine(IContext globalContext)
        {
            _changeStateRequester = globalContext.Get<GameStateMachineChangeStateRequester>();
            _isBusy = true;
        }
        
        public void AddState(GameState state)
        {
            _states.Add(state.GetType(), state);
        }

        public async UniTask SetInitialState(Type initialStateType)
        {
            _isBusy = true;
            
            var initialState = _states[initialStateType];
            
            foreach (var initializer in initialState.Initializers)
            {
                await initializer.Init(initialState.Context);
            }

            foreach (var feature in initialState.Features)
            {
                await feature.Init(initialState.Context);
            }
            
            _currentState = initialState;
            
            _isBusy = false;
        }

        public void Tick(float deltaTime)
        {
            if (_isBusy)
            {
                return;
            }
            
            _currentState.Tick(deltaTime);

            ProcessStateChangeRequest();
        }
        
        public void FixedTick(float deltaTime)
        {
            if (_isBusy)
            {
                return;
            }
            
            _currentState.FixedTick(deltaTime);
            
            ProcessStateChangeRequest();
        }
        
        public void LateTick(float deltaTime)
        {
            if (_isBusy)
            {
                return;
            }
            
            _currentState.LateTick(deltaTime);
            
            ProcessStateChangeRequest();
        }
        
        private void ProcessStateChangeRequest()
        {
            if (!_changeStateRequester.IsRequested)
            {
                return;
            }
            
            var newStateType = _changeStateRequester.RequestedState!;
            
            SwitchState(newStateType).Forget();
            
            _changeStateRequester.SetCompleted();
        }

        private async UniTask SwitchState(Type newStateType)
        {
            _isBusy = true;
            
            var previousState = _currentState;
            var nextState = _states[newStateType];
            
            // Deinit all features that are not present in the next state
            foreach (var initializer in previousState.Initializers)
            {
                if (nextState.HasFeature(initializer.GetType()))
                {
                    continue;
                }
                
                initializer.DeInit();
            }
            
            foreach (var feature in previousState.Features)
            {
                if (nextState.HasFeature(feature.GetType()))
                {
                    continue;
                }
                
                feature.DeInit();
            }
            
            // Refresh all initializers that are present in the next state
            foreach (var initializer in previousState.Initializers)
            {
                if (!nextState.HasFeature(initializer.GetType()))
                {
                    continue;
                }
                
                await initializer.Refresh(previousState.Context, nextState.Context);
            }

            // Init all features that are not present in the previous state
            foreach (var initializer in nextState.Initializers)
            {
                if (previousState.HasFeature(initializer.GetType()))
                {
                    continue;
                }
                
                await initializer.Init(nextState.Context);
            }
            
            foreach (var feature in nextState.Features)
            {
                if (previousState.HasFeature(feature.GetType()))
                {
                    continue;
                }
                
                await feature.Init(nextState.Context);
            }
            
            // Refresh all features that are present in the next state with the new context
            foreach (var feature in previousState.Features)
            {
                if (!nextState.HasFeature(feature.GetType()))
                {
                    continue;
                }
                
                await feature.Refresh(previousState.Context, nextState.Context);
            }
            
            _currentState = nextState;
            _isBusy = false;
        }
    }
}