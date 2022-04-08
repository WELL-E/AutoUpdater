using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Events
{
    /// <summary>
    /// Manage all events in the component.
    /// </summary>
    public sealed class GeneralEventManager
    {
        private static readonly object _lockObj = new object();
        private static GeneralEventManager _instance;
        private Dictionary<Type, Delegate> _dicDelegates = new Dictionary<Type, Delegate>();

        private GeneralEventManager() { }

        public static GeneralEventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new GeneralEventManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddListener<TDelegate>(TDelegate newDelegate) where TDelegate : Delegate
        {
            if (_dicDelegates.ContainsKey(typeof(TDelegate))) return;
            _dicDelegates.Add(typeof(TDelegate), newDelegate);
        }

        public void RemoveListener<TDelegate>(TDelegate oldDelegate) where TDelegate : Delegate
        {
            var delegateType = oldDelegate.GetType();
            if (!delegateType.IsInstanceOfType(typeof(TDelegate))) return;
            Delegate tempDelegate = null;
            if (_dicDelegates.TryGetValue(delegateType, out tempDelegate))
            {
                if (tempDelegate == null)
                {
                    _dicDelegates.Remove(delegateType);
                }
                else
                {
                    _dicDelegates[delegateType] = tempDelegate;
                }
            }
        }

        public void Dispatch<TDelegate>(EventArgs eventArgs) where TDelegate : Delegate 
        {
            if (!_dicDelegates.ContainsKey(typeof(TDelegate))) return;
            _dicDelegates[typeof(TDelegate)].DynamicInvoke(eventArgs);
        }

        public void Clear()=> _dicDelegates.Clear();
    }
}
