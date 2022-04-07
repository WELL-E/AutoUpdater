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

        public delegate void EventDelegate<T>(T e);
        private delegate void EventDelegate(EventArgs args);

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

        public void AddListener<TEventDelegate, TEventArgs>(TEventDelegate listener, TEventArgs args) where TEventArgs : EventArgs , TEventDelegate
        {
        }

        public void RemoveListener() { }

        public void Dispatch() { }

        public void Clear()=> _dicDelegates.Clear();
    }
}
