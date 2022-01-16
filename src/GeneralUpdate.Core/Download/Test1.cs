using GeneralUpdate.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal class Test1
    {
        public Test1() 
        {
            var manger = new DownloadManager("","");
            manger.EnPool(new DownloadTask<UpdateVersion>(manger, new UpdateVersion("", 1, "", "", "")));
            manger.EnPool(new DownloadTask<UpdateVersion>(manger, new UpdateVersion("", 1, "", "", "")));
            manger.Launch();
        }
    }
}
