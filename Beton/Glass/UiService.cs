using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Beton.Core.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Beton.Glass
{
    public abstract class UiServiceBase<TWindowConfig> : IServiceWithGameStateContext 
        where TWindowConfig : class, IWindowConfig
    {
        public IReadOnlyContext GameStateContext { get; set; }
        
        protected readonly IReadOnlyContext Context;
        protected readonly TWindowConfig WindowsConfigs;
        protected readonly Dictionary<WindowId, IWindow> SpawnedWindows = new();

        protected UiServiceBase(IReadOnlyContext context)
        {
            Context = context;
            WindowsConfigs = context.Get<TWindowConfig>();
        }
        
        public bool IsSpawned(WindowId id)
        {
            return SpawnedWindows.ContainsKey(id);
        }
        
        public async UniTask<WindowId> CreateWindow<TWindow, TWindowView, TWindowViewModel, TWindowData>(
            TWindowData data, AssetReferenceGameObject windowPrefab) 
            where TWindowViewModel : WindowViewModel<TWindowData>, new() 
            where TWindowView : WindowView<TWindowViewModel>
            where TWindow : WindowBase<TWindowView, TWindowViewModel, TWindowData>, new()
        {
            var id = new WindowId();
            var handle = windowPrefab.InstantiateAsync();
            handle.Completed += h =>
            {
                if (h.Status == AsyncOperationStatus.Succeeded)
                {
                    var view = h.Result.GetComponent<TWindowView>();
                    view.CanvasGroup.alpha = 0;
                }
            };
            var go = await handle.Task;
            
            var viewModel = new TWindowViewModel();
            viewModel.Construct(GameStateContext);
            viewModel.CloseWindow = () => DestroyWindow<TWindow, TWindowView, TWindowViewModel, TWindowData>(id);
            
            var view = go.GetComponent<TWindowView>();
            view.Construct(Context);
            
            var window = new TWindow();
            window.Construct(view, viewModel);
            SpawnedWindows.Add(id, window);
            
            if (window.WindowActivator != null)
            {
                await window.WindowActivator.OnSpawn();
            }
            
            viewModel.Init(data);
            view.Init(viewModel);
           
            if (window.WindowActivator != null)
            {
                await window.WindowActivator.Show();
            }
            else
            {
                view.CanvasGroup.alpha = 1;
            }
            
            return id;
        }
        
        public async void DestroyWindow<TWindow, TWindowView, TWindowViewModel, TWindowData>(WindowId id)
            where TWindowViewModel : WindowViewModel<TWindowData>, new() 
            where TWindowView : WindowView<TWindowViewModel>
            where TWindow : WindowBase<TWindowView, TWindowViewModel, TWindowData>, new()
        {
            if (id == null)
            {
                return;
            }
            
            if (!SpawnedWindows.TryGetValue(id, out var windowBase))
            {
                return;
            }
            
            if (windowBase is not TWindow window)
            {
                return;
            }
            
            if (window.WindowActivator != null)
            {
                await window.WindowActivator.Hide();
            }
            
            window.View.Deinit();
            window.View.Dispose();
            
            window.ViewModel.Deinit();
            window.ViewModel.Dispose();
            
            Addressables.Release(window.View.gameObject);
            
            SpawnedWindows.Remove(id);
        }
    }
}