using System;
using Beton.Beton.Core.Services;
using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.GameStates
{
    public abstract class ServiceInitializer : IDisposable
    {
        protected IService Service { get; set; }
        protected IContext Context { get; set; }
        
        public void SetGlobalContext(IContext globalContext)
        {
            Context = globalContext;
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