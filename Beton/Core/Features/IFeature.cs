using System;
using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.Features
{
    public interface IFeature : IDisposable
    {
        UniTask Init(IContext stateContext);
        UniTask Refresh(IContext prevStateContext, IContext newStateContext);
        void DeInit();
    }
}