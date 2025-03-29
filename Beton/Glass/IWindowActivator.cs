using Cysharp.Threading.Tasks;

namespace Beton.Glass
{
    public interface IWindowActivator
    {
        public UniTask OnSpawn();
        public UniTask Show();
        public UniTask Hide();
    }
}