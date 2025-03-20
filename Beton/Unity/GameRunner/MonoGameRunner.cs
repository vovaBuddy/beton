using Beton.Core.GameRunner;
using UnityEngine;

namespace Beton.Unity.GameRunner
{
    public abstract class MonoGameRunner<TGameRunner> : MonoBehaviour 
        where TGameRunner : GameRunnerBase, new()
    {
        private TGameRunner _gameRunner;

        private void Awake()
        {
            _gameRunner = new TGameRunner();
            _gameRunner.SetupStates();
        }

        private void Update()
        {
            _gameRunner.Tick(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _gameRunner.FixedTick(Time.fixedDeltaTime);
        }
        
        private void LateUpdate()
        {
            _gameRunner.LateTick(Time.deltaTime);
        }
        
        private void OnDestroy()
        {
            _gameRunner?.Dispose();
        }
    }
}