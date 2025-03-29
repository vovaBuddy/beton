using System;

namespace Beton.Glass
{
    public abstract class WindowViewModel<TData> : ViewModel<TData>, IWindowViewModel
    {
        public Action CloseWindow { get; set; }
    }
}