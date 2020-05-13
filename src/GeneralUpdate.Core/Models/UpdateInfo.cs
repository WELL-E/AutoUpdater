using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Models
{
    public class UpdateInfoHttpResp
    {
        public UpdateInfo Result { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }
    }

    public class UpdateInfo
    {
        public string Version { get; set; }

        public DateTime PubTime { get; set; }

        public int Mandatory { get; set; }

        public string Url { get; set; }

        public string HashCode { get; set; }

        public string Log { get; set; }
    }
}
