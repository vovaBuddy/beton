using System;
using Cysharp.Threading.Tasks;

namespace Beton.Glass
{
    public abstract class WindowViewModel<TData> : ViewModel, IWindowViewModel
    {
        public Action CloseWindow { get; set; }
        public abstract void Init(TData data);
    }
}