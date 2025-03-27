using System;
using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Beton.Core.DependencyInjections.Attributes;
using Beton.Core.Services;
using Beton.Unity.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Beton.Glass
{
    [Config]
    public class WindowsConfigs
    {
        public AssetReferenceGameObject DemoWindowPrefab;
    }

    public sealed class WindowId
    {
        
    }
    
    public abstract class UiServiceBase : IService
    {
        public IReadOnlyContext CurrentGameStateContext { get; set; }
        
        [Inject] protected WindowsConfigs WindowsConfigs;
        
        protected Dictionary<WindowId, IWindow> SpawnedWindows = new();
        protected IReadOnlyContext Context;
        
    }
    
    
    public partial class UiService : UiServiceBase
    {
    }

    public partial class UiService
    {
        public async UniTask<WindowId> CreateDemoWindow(DemoWindowData data)
        {
            var id = new WindowId();

            var go = await WindowsConfigs.DemoWindowPrefab.InstantiateAsync().Task;
            
            var viewModel = new DemoWindowViewModel(CurrentGameStateContext);
            var view = go.GetComponent<DemoWindowView>();
            view.gameObject.SetActive(false);
            view.Construct(Context);
            SpawnedWindows.Add(id, new DemoWindow(view, viewModel));
                
            viewModel.Init(data);
            //todo: тут можно добавить анимацию появления
            view.gameObject.SetActive(true);
            
            return id;
        }
        
        public void DestroyDemoWindow(WindowId id)
        {
            if (!SpawnedWindows.TryGetValue(id, out var window))
            {
                return;
            }
            
            if (window is not DemoWindow demoWindow)
            {
                return;
            }
            
            //ToDo: можно обернуть все в анимацию закрытия, и в конце вызвать Deinit и Dispose
            
            demoWindow.View.Deinit();
            demoWindow.View.Dispose();
            
            demoWindow.ViewModel.Deinit();
            demoWindow.ViewModel.Dispose();
            
            Addressables.Release(demoWindow.View.gameObject);
            
            SpawnedWindows.Remove(id);
        }
    }

    public abstract class View : MonoBehaviour
    {
        protected IReadOnlyContext Context;
        
        protected virtual void InjectDependencies() { }

        public void Construct(IReadOnlyContext context)
        {
            Context = context;
            
            InjectDependencies();
        }
        
        public virtual void Deinit()
        {
            
        }
        
        public virtual void Dispose()
        {
            
        }
    }

    public abstract class ViewModel
    {
        protected IReadOnlyContext Context;
        
        protected virtual void InjectDependencies() { }

        protected ViewModel(IReadOnlyContext context)
        {
            Context = context;
            
            InjectDependencies();
        }

        public virtual void Deinit()
        {
            
        }
        
        public virtual void Dispose()
        {
            
        }
    }

    public interface IWindow
    {
        
    }

    public abstract class WindowBase<TView, TViewModel> : IWindow
        where TView : View where TViewModel : ViewModel
    {
        public TView View { get; set; }
        public TViewModel ViewModel { get; set; }

        protected WindowBase(TView view, TViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }
    }
    
    public class DemoWindowView : View
    {

    }

    public struct DemoWindowData
    {
        
    }
    
    public class DemoWindowViewModel : ViewModel
    {
        public DemoWindowViewModel(IReadOnlyContext context) : base(context)
        {
        }

        public void Init(DemoWindowData data)
        {
            
        }
    }
    
    public class DemoWindow : WindowBase<DemoWindowView, DemoWindowViewModel>
    {
        public DemoWindow(DemoWindowView view, DemoWindowViewModel viewModel) : base(view, viewModel)
        {
        }
    }
}