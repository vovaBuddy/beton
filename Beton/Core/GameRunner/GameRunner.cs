using System;
using Beton.Core.DependencyInjections;
using Beton.Core.Features;
using Beton.Core.GameStates;

namespace Beton.Core.GameRunner
{
    public abstract class GameRunnerBase : IDisposable
    {
        protected readonly GameStateMachine _gameStateMachine;
        protected readonly IContext _globalContext;
        
        protected readonly FeaturesStorage _featuresStorage = new();
        
        private readonly GameStateMachineChangeStateRequester _changeStateRequester = new();

        protected GameRunnerBase()
        {
            _globalContext = new Context(null);
            _globalContext.Set(_changeStateRequester);
            _gameStateMachine = new GameStateMachine(_globalContext);
        }
        
        public void Tick(float deltaTime)
        {
            _gameStateMachine.Tick(deltaTime);
        }
        
        public void FixedTick(float deltaTime)
        {
            _gameStateMachine.FixedTick(deltaTime);
        }
        
        public void LateTick(float deltaTime)
        {
            _gameStateMachine.LateTick(deltaTime);
        }

        public abstract void SetupStates();

        public void Dispose()
        {
            _globalContext?.Dispose();
        }
    }
}