using System;
using Beton.Core.DependencyInjections;
using Beton.Core.Features;
using Beton.Core.GameStates;
using UnityEngine;

namespace Beton.Unity.GameRunner
{
    public abstract class MonoGameRunner : MonoBehaviour 
    {
        protected GameStateMachine _gameStateMachine;
        protected IContext _globalContext;
        
        protected FeaturesStorage _featuresStorage = new();
        
        private GameStateMachineChangeStateRequester _changeStateRequester = new();
        
        protected abstract void SetupStates();

        private void Construct()
        {
            _globalContext = new Context(null);
            _globalContext.Set(_changeStateRequester);
            _gameStateMachine = new GameStateMachine(_globalContext);
        }

        private void Awake()
        {
            Construct();
            SetupStates();
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