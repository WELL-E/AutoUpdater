using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GeneralUpdate.ClientCore.Utils
{
    public class DataValidateUtil
    {
        public static bool IsURL(string url)
        {
            string check = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
            var regex = new Regex(check);
            return regex.IsMatch(url);
        }
    }
}
