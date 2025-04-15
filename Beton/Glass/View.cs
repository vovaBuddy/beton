using System;
using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Beton.Reactive;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Beton.Glass
{
    public abstract class View<TViewModel> : MonoBehaviour, IView
    {
        protected IReadOnlyContext Context;
        private List<Action> _unbindActions = new();
        
        protected virtual void InjectDependencies() { }
        protected virtual void OnInit(TViewModel viewModel) { }
        internal virtual void OnInitInternal(TViewModel viewModel) { }
        protected virtual void ConstructChildren() { }
        protected virtual void OnConstruct() { }
        protected virtual void DeinitChildren() { }
        protected virtual void OnDeinit() { }
        protected virtual void DisposeChildren() { }
        protected virtual void OnDispose() { }

        public void Construct(IReadOnlyContext context)
        {
            Context = context;
            
            InjectDependencies();
            
            ConstructChildren();
            
            OnConstruct();
        }
        
        public void Init(TViewModel viewModel)
        {
            OnInitInternal(viewModel);
            
            OnInit(viewModel);
        }
        
        public void Deinit()
        {
            OnDeinit();
            
            DeinitChildren();
            
            UnbindAll();
        }
        
        public void Dispose()
        {
            UnbindAll();
            
            OnDispose();
            
            DisposeChildren();
        }
        
        public void UnbindAll()
        {
            for(var i = _unbindActions.Count - 1; i >= 0; i--)
            {
                _unbindActions[i]();
            }
            
            _unbindActions.Clear();
        }
        
        protected void Bind(Button button, ReactiveCommand action)
        {
            button.onClick.AddListener(action.Invoke);
            _unbindActions.Add(() => button.onClick.RemoveListener(action.Invoke));
        }
        
        protected void Bind(List<Button> buttons, ReactiveCommand action)
        {
            foreach (var button in buttons)
            {
                Bind(button, action);
            }
        }
        
        protected void Bind(Toggle toggle, Action callback, bool setOn = false)
        {
            void ToggleCallback(bool isOn)
            {
                if (isOn)
                {
                    callback.Invoke();
                }
            }
            
            toggle.onValueChanged.AddListener(ToggleCallback);
            _unbindActions.Add(() => toggle.onValueChanged.RemoveListener(ToggleCallback));

            if (setOn)
            {
                toggle.SetIsOnWithoutNotify(true);
                toggle.onValueChanged.Invoke(true);
            }
        }

        protected void Bind(ReactiveCommand action, Action callback)
        {
            action.Subscribe(callback);
            _unbindActions.Add(() => action.Unsubscribe(callback));
        }
        
        protected void Bind(List<Button> buttons, Action action)
        {
            foreach (var button in buttons)
            {
                Bind(button, action);
            }
        }
        
        protected void Bind<TData>(ReactiveCommand<TData> action, Action<TData> callback)
        {
            action.Subscribe(callback);
            _unbindActions.Add(() => action.Unsubscribe(callback));
        }
        
        protected void Bind<TData>(ReactiveData<TData> action, Action<TData> callback, bool invokeImmediately = true)
        {
            action.Subscribe(callback, invokeImmediately);
            _unbindActions.Add(() => action.Unsubscribe(callback));
        }
        
        protected void Bind(Button button, Action action)
        {
            var delegateAction = new UnityAction(action);
            button.onClick.AddListener(delegateAction);
            _unbindActions.Add(() => button.onClick.RemoveListener(delegateAction));
        }
        
        protected void Bind(ReactiveData<Sprite> data, Image image)
        {
            void SetImage(Sprite sprite) => image.sprite = sprite;
            
            data.Subscribe(SetImage);
            _unbindActions.Add(() => data.Unsubscribe(SetImage));
        }
        
        protected void BindAdd<TData>(ReactiveList<TData> list, Action<TData> addCallback)
        {
            list.SubscribeAdd(addCallback);
            _unbindActions.Add(() => list.UnsubscribeAdd(addCallback));
        }
        
        protected void BindRemove<TData>(ReactiveList<TData> list, Action<TData> removeCallback)
        {
            list.SubscribeRemove(removeCallback);
            _unbindActions.Add(() => list.UnsubscribeRemove(removeCallback));
        }
    }
}