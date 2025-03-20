#nullable enable

using System;
using System.Collections.Generic;

namespace Beton.Core.DependencyInjections
{
    public class Context : IContext
    {
        public IContext? Parent { get; set; }
        
        private readonly Dictionary<Type, object> _dependencies;

        public Context(IContext? parent)
        {
            Parent = parent;
            _dependencies = new Dictionary<Type, object>();
        }

        public T? Get<T>() where T : class
        {
            var key = typeof(T);
            if (_dependencies.TryGetValue(key, out var dependency))
            {
                return (T)dependency;
            }
            
            return Parent?.Get<T>();
        }
        
        public void Set<T>(T dependency) where T : class
        {
            _dependencies[typeof(T)] = dependency;
        }

        public void Set(Type type, object dependency)
        {
            _dependencies[type] = dependency;
        }

        public void Remove<T>() where T : class
        {
            _dependencies.Remove(typeof(T));
        }

        public void Remove(Type type)
        {
            _dependencies.Remove(type);
        }

        public void Dispose()
        {
            foreach (var dependency in _dependencies.Values)
            {
                if (dependency is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            _dependencies.Clear();
        }
    }
}