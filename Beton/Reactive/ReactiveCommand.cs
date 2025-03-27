using System;
using System.Collections.Generic;

namespace Beton.Reactive
{
    public class ReactiveCommand
    {
        private List<Action> _subscribers = new(1);
        
        public void Subscribe(Action action)
        {
            _subscribers.Add(action);
        }
        
        public void Unsubscribe(Action action)
        {
            _subscribers.Remove(action);
        }
        
        public void Invoke()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber();
            }
        }
    }
    
    public class ReactiveCommand<T>
    {
        private List<Action<T>> _subscribers = new(1);
        
        public void Subscribe(Action<T> action)
        {
            _subscribers.Add(action);
        }
        
        public void Unsubscribe(Action<T> action)
        {
            _subscribers.Remove(action);
        }
        
        public void Invoke(T data)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(data);
            }
        }
    }
}