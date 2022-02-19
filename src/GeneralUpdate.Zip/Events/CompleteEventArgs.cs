using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.Events
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
