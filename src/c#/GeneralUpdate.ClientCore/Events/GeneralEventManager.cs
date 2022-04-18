using System;
using System.Collections.Generic;

namespace GeneralUpdate.Core.Events
{
    /// <summary>
    /// Manage all events in the component.
    /// </summary>
    public sealed class GeneralEventManager : IGeneralEventManager
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

        /// <summary>
        /// Add listener
        /// </summary>
        /// <typeparam name="TDelegate">Specify the delegate type.</typeparam>
        /// <param name="newDelegate">Delegate to be added.</param>
        /// <exception cref="ArgumentNullException">parameter null exception.</exception>
        public void AddListener<TDelegate>(TDelegate newDelegate) where TDelegate : Delegate
        {
            if (newDelegate == null) throw new ArgumentNullException(nameof(newDelegate));
            if (_dicDelegates.ContainsKey(typeof(TDelegate))) return;
            _dicDelegates.Add(typeof(TDelegate), newDelegate);
        }

        /// <summary>
        /// Remove listener
        /// </summary>
        /// <typeparam name="TDelegate">Specify the delegate type.</typeparam>
        /// <param name="oldDelegate">Remove old delegates.</param>
        /// <exception cref="ArgumentNullException">parameter null exception.</exception>
        public void RemoveListener<TDelegate>(TDelegate oldDelegate) where TDelegate : Delegate
        {
            if (oldDelegate == null) throw new ArgumentNullException(nameof(oldDelegate));
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

        /// <summary>
        /// triggers a delegate of the same type.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="sender">trigger source object.</param>
        /// <param name="eventArgs">event args.</param>
        /// <exception cref="ArgumentNullException">parameter null exception.</exception>
        public void Dispatch<TDelegate>(object sender ,EventArgs eventArgs) where TDelegate : Delegate 
        {
            if(sender == null)throw new ArgumentNullException(nameof(sender));
            if(eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));
            if (!_dicDelegates.ContainsKey(typeof(TDelegate))) return;
            _dicDelegates[typeof(TDelegate)].DynamicInvoke(sender , eventArgs);
        }

        /// <summary>
        /// clear all listeners.
        /// </summary>
        public void Clear()=> _dicDelegates.Clear();
    }
}
