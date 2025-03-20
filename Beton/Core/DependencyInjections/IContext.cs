#nullable enable

using System;

namespace Beton.Core.DependencyInjections
{
    public interface IContext : IReadOnlyContext, IDisposable
    {
        void Set<T>(T dependency) where T : class;
        void Set(Type type, object dependency);
        void Remove<T>() where T : class;
        void Remove(Type type);
    }
    
    public interface IReadOnlyContext
    {
        T Get<T>() where T : class;
    }
}