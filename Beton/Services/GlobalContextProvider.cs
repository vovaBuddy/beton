using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Services
{
    public static class GlobalContextProvider
    {
        public static bool IsReady { get; private set; } = false;
        public static IContext GlobalContext { get; private set; } = null;
        
        private static readonly UniTaskCompletionSource _readyTaskCompletionSource = new();
    
        public static void SetGlobalContext(IContext globalContext)
        {
            GlobalContext = globalContext;
            IsReady = true;
            _readyTaskCompletionSource.TrySetResult(); 
        }
        
        public static UniTask.Awaiter GetAwaiter()
        {
            return AwaitReady().GetAwaiter();
        }

        private static UniTask AwaitReady()
        {
            return IsReady 
                ? UniTask.CompletedTask 
                : _readyTaskCompletionSource.Task;
        }
    }
}