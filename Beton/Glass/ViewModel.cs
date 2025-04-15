using Beton.Core.DependencyInjections;

namespace Beton.Glass
{
    public abstract class ViewModel : IViewModel
    {
        protected IReadOnlyContext Context;
        
        protected virtual void InjectDependencies() { }
        protected virtual void ConstructChildren() { }
        protected virtual void OnConstruct() { }
        protected virtual void DeinitChildren() { }
        protected virtual void OnDeinit() { }
        protected virtual void DisposeChildren() { }
        protected virtual void OnDispose() { }
        

        public void Construct(IReadOnlyContext context)
        {
            Context = context;
            
            InjectDependencies();
            
            ConstructChildren();

            OnConstruct();
        }

        public void Deinit()
        {
            OnDeinit();
            
            DeinitChildren();
        }
        
        public void Dispose()
        {
            OnDispose();
            
            DisposeChildren();
        }
    }
}