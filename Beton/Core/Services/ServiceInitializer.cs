using System;
using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.Services
{
    public abstract class ServiceInitializer : IDisposable
    {
        protected IService Service
        {
            get => _service;
            set
            {
                _service = value;
                if(_service is IServiceWithGameStateContext serviceWithGameStateContext)
                {
                    _serviceWithGameStateContext = serviceWithGameStateContext;
                }
            }
        }

        protected IContext Context { get; set; }

        private IService _service;
        private IServiceWithGameStateContext _serviceWithGameStateContext;
        
        public void SetGlobalContext(IContext globalContext)
        {
            Context = globalContext;
        }
        
        public void TrySetGameStateContext(IReadOnlyContext gameStateContext)
        {
            if(_serviceWithGameStateContext != null)
            {
                _serviceWithGameStateContext.GameStateContext = gameStateContext;
            }
        }
        
        protected virtual void InjectDependencies() { }

        protected virtual UniTask OnInit()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnDispose() { }

        public async UniTask Init()
        {
            InjectDependencies();
                
            await OnInit();
            
            Context.Set(Service.GetType(), Service);

            if (Service == null)
            {
                throw new NullReferenceException($"[{GetType()}] Service is not assigned");
            }
        }

        public void Dispose()
        {
            OnDispose();
            
            Service = null;
        }
    }
}