using System;

namespace GeneralUpdate.Core.Events
{
    internal interface IGeneralEventManager
    {
        void AddListener<TDelegate>(TDelegate newDelegate) where TDelegate : Delegate;

        void RemoveListener<TDelegate>(TDelegate oldDelegate) where TDelegate : Delegate;

        void Dispatch<TDelegate>(object sender, EventArgs eventArgs) where TDelegate : Delegate;

        void Clear();
    }
}
