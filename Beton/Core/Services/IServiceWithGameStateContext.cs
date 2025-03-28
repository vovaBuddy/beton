using Beton.Core.DependencyInjections;

namespace Beton.Core.Services
{
    public interface IServiceWithGameStateContext : IService
    {
        IReadOnlyContext GameStateContext { get; set; }
    }
}