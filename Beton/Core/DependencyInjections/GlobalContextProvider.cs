﻿using Beton.Core.DependencyInjections;
using Cysharp.Threading.Tasks;

namespace Beton.Core.DependencyInjections
{
    public static class GlobalContextProvider
    {
        public static bool IsReady { get; private set; } = false;
        public static IReadOnlyContext GlobalContext { get; private set; } = null;
        
        private static readonly UniTaskCompletionSource _readyTaskCompletionSource = new();
    
        public static void SetGlobalContext(IContext globalContext)
        {
            GlobalContext = globalContext;
            IsReady = true;
            _readyTaskCompletionSource.TrySetResult(); 
        }
        
        public static UniTask AwaitIsReady()
        {
            return IsReady 
                ? UniTask.CompletedTask 
                : _readyTaskCompletionSource.Task; 
        }
    }
}