using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.Features
{
    public abstract class Feature : IFeature
    {
        protected IReadOnlyContext Context { get; private set; }

        protected virtual async UniTask OnInit()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnRefresh()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual void OnDeInit() { }
        protected virtual void OnDispose() { }
        

        public async UniTask Init(IContext stateContext)
        {
            Context = stateContext;
            
            await OnInit();
        }

        public async UniTask Refresh(IContext prevStateContext, IContext newStateContext)
        {
            Context = newStateContext;
            
            await OnRefresh();
        }

        public void DeInit()
        {
            OnDeInit();
            
            Context = null;
        }
        
        public void Dispose()
        {
            OnDispose();
        }
    }
}