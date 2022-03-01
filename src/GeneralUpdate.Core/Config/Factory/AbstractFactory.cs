using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Config.Factory
{
    public abstract class AbstractFactory : IFactory, IDisposable
    {
        public virtual void Backup()=> throw new NotImplementedException();

        public virtual void Cache()=> throw new NotImplementedException();

        public virtual void Deploy()=> throw new NotImplementedException();

        public virtual void Dispose() => throw new NotImplementedException();

        public virtual IEnumerable<string> Scan(string path, IEnumerable<string> configFormat)=> throw new NotImplementedException();

        public virtual bool Verify()=> throw new NotImplementedException();
    }
}
