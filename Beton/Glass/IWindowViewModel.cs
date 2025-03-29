using System;

namespace Beton.Glass
{
    public interface IWindowViewModel
    {
        Action CloseWindow { get; set; }
    }
}