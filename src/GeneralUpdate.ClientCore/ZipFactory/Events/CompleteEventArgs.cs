using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.ClientCore.ZipFactory.Events
{
    public class CompleteEventArgs
    {
        public bool IsCompleted { get; set; }

        public CompleteEventArgs(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }
    }
}
