using Beton.Core.DependencyInjections;

namespace Beton.Glass
{
    public abstract class ViewModel<TData>
    {
        protected IReadOnlyContext Context;
        
        protected virtual void InjectDependencies() { }
        protected virtual void OnConstruct() { }

        public void Construct(IReadOnlyContext context)
        {
            Context = context;
            
            InjectDependencies();

            OnConstruct();
        }
        
        public virtual void Init(TData data)
        {
            
        }

        public virtual void Deinit()
        {
            
        }
        
        public virtual void Dispose()
        {
            
        }
    }
}