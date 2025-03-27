using System;
using System.Collections;
using System.Collections.Generic;

namespace Beton.Reactive
{
    public class ReactiveList<TData> : IEnumerable<TData>
    {
        private List<TData> _list;
        private List<Action<TData>> _onAddSubscribers;
        private List<Action<TData>> _onRemoveSubscribers;
        
        public ReactiveList(int capacity = 0, int subscribersCapacity = 0)
        {
            _list = new List<TData>(capacity);
            _onAddSubscribers = new List<Action<TData>>(subscribersCapacity);
            _onRemoveSubscribers = new List<Action<TData>>(subscribersCapacity);
        }
        
        public void Add(TData data)
        {
            _list.Add(data);
            
            foreach (var subscriber in _onAddSubscribers)
            {
                subscriber.Invoke(data);
            }
        }
        
        public void Remove(TData data)
        {
            _list.Remove(data);
            
            foreach (var subscriber in _onRemoveSubscribers)
            {
                subscriber.Invoke(data);
            }
        }
        
        public void Clear()
        {
            foreach (var data in _list)
            {
                foreach (var subscriber in _onRemoveSubscribers)
                {
                    subscriber.Invoke(data);
                }
            }
            
            _list.Clear();
        }
        
        public void SubscribeAdd(Action<TData> action, bool invokeOnSubscribe = true)
        {
            _onAddSubscribers.Add(action);
            
            if (invokeOnSubscribe)
            {
                foreach (var data in _list)
                {
                    action.Invoke(data);
                }
            }
        }
        
        public void UnsubscribeAdd(Action<TData> action)
        {
            _onAddSubscribers.Remove(action);
        }
        
        public void SubscribeRemove(Action<TData> action)
        {
            _onRemoveSubscribers.Add(action);
        }
        
        public void UnsubscribeRemove(Action<TData> action)
        {
            _onRemoveSubscribers.Remove(action);
        }

        public IEnumerator<TData> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}