using System;
using System.Globalization;

namespace GeneralUpdate.Core.Utils
{
    internal static class StatisticsUtil
    {
        internal static string ToUnit(long byteSize)
        {
            string str;
            var tempSize = Convert.ToSingle(byteSize);
            if (tempSize / 1024 > 1)
            {
                if ((tempSize / 1024) / 1024 > 1)
                {
                    str = $"{((tempSize / 1024) / 1024).ToString("##0.00", CultureInfo.InvariantCulture)}MB/S";
                }
                else
                {
                    str = $"{(tempSize / 1024).ToString("##0.00", CultureInfo.InvariantCulture)}KB/S";
                }
            }
            else
            {
                str = $"{tempSize.ToString(CultureInfo.InvariantCulture)}B/S";
            }
            return str;
        }
    }
}
