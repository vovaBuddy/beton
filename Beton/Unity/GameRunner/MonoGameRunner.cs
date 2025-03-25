using System;
using Beton.Core.DependencyInjections;
using Beton.Core.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Beton.Unity.GameRunner
{
    public abstract class MonoGameRunner : MonoBehaviour 
    {
        private GameStateMachine _gameStateMachine;
        private IContext _globalContext;
        
        private GameStateMachineChangeStateRequester _changeStateRequester = new();
        
        protected abstract Type SetupStates();

        private void Construct()
        {
            _globalContext = new Context(null);
            _globalContext.Set(_changeStateRequester);
            _gameStateMachine = new GameStateMachine(_globalContext);
        }
        
        protected void AddState<TState>() where TState : GameState
        {
            _gameStateMachine.AddState<TState>();
        }
        
        protected void AddServiceInitializer<TServiceInitializer>() where TServiceInitializer : ServiceInitializer, new()
        {
            _gameStateMachine.AddServiceInitializer<TServiceInitializer>();
        }

        private void Awake()
        {
            Construct();
            var initialStateType = SetupStates();
            _gameStateMachine.SetInitialState(initialStateType).Forget();
        }

        private void Update()
        {
            _gameStateMachine.Tick(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _gameStateMachine.FixedTick(Time.deltaTime);
        }
        
        private void LateUpdate()
        {
            _gameStateMachine.LateTick(Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            _globalContext?.Dispose();
        }
    }
}