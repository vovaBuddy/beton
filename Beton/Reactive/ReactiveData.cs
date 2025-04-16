using System;
using System.Collections.Generic;

namespace Beton.Reactive
{
    public class ReactiveData<TData>
    {
        private List<Action<TData>> _subscribers = new(1);
        
        private TData _data;
        public TData Value
        {
            get => _data;
            set
            {
                _data = value;
                Invoke();
            }
        }
        
        public ReactiveData() { }
        
        public ReactiveData(TData data)
        {
            _data = data;
        }
        
        public void SetWithoutNotify(TData data)
        {
            _data = data;
        }
        
        public void Notify()
        {
            Invoke();
        }
        
        public void Subscribe(Action<TData> action, bool invokeImmediately = true)
        {
            _subscribers.Add(action);
            if (invokeImmediately)
            {
                action(_data);
            }
        }
        
        public void Unsubscribe(Action<TData> action)
        {
            _subscribers.Remove(action);
        }
        
        private void Invoke()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(_data);
            }
        }
    }
}