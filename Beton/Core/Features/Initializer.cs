using System;
using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.Features
{
    public abstract class Initializer : IFeature
    {
        protected IReadOnlyContext Context => _context;

        private IContext _context;
        private Dictionary<Type, object> _initializables = new();
        
        protected virtual async UniTask OnInit()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask InitializeDependencies()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnRefresh()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual void OnDeInit() { }
        protected virtual void OnDispose() { }
        
        protected void Initialize<T>(T obj)
        {
            _initializables.Add(typeof(T), obj);
        }
        
        public async UniTask Init(IContext stateContext)
        {
            _context = stateContext;

            await InitializeDependencies();
            
            await OnInit();
            
            foreach (var (type, obj) in _initializables)
            {
                stateContext.Set(type, obj);
            }
        }

        public async UniTask Refresh(IContext prevStateContext, IContext newStateContext)
        {
            _context = newStateContext;
            
            await OnRefresh();
            
            foreach (var (type, obj) in _initializables)
            {
                prevStateContext.Remove(type);
                newStateContext.Set(type, obj);
            }
        }

        public void DeInit()
        {
            OnDeInit();
            
            foreach (var (type, obj) in _initializables)
            {
                _context.Remove(type);
            }
            
            _initializables.Clear();
        }

        public void Dispose()
        {
            OnDispose();
        }
    }
}