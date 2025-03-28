using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Beton.Core.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

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
        
        public async UniTask<WindowId> CreateWindow<TWindow, TWindowView, TWindowViewModel, TWindowData>(
            TWindowData data, AssetReferenceGameObject windowPrefab) 
            where TWindowViewModel : ViewModel<TWindowData>, new() 
            where TWindowView : View<TWindowViewModel>
            where TWindow : WindowBase<TWindowView, TWindowViewModel, TWindowData>, new()
        {
            var id = new WindowId();
            var go = await windowPrefab.InstantiateAsync().Task;
            
            var viewModel = new TWindowViewModel();
            viewModel.Construct(GameStateContext);
            
            var view = go.GetComponent<TWindowView>();
            view.gameObject.SetActive(false);
            view.Construct(Context);

            var window = new TWindow();
            window.Construct(view, viewModel);
            SpawnedWindows.Add(id, window);
                
            viewModel.Init(data);
            view.Init(viewModel);
            //todo: тут можно добавить анимацию появления
            view.gameObject.SetActive(true);
            
            return id;
        }
        
        public void DestroyWindow<TWindow, TWindowView, TWindowViewModel, TWindowData>(WindowId id)
            where TWindowViewModel : ViewModel<TWindowData>, new() 
            where TWindowView : View<TWindowViewModel>
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
            
            //ToDo: можно обернуть все в анимацию закрытия, и в конце вызвать Deinit и Dispose
            
            window.View.Deinit();
            window.View.Dispose();
            
            window.ViewModel.Deinit();
            window.ViewModel.Dispose();
            
            Addressables.Release(window.View.gameObject);
            
            SpawnedWindows.Remove(id);
        }
    }
}