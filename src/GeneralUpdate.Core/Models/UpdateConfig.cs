using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Models
{
    public class UpdateConfig
    {
        /// <summary>
        /// 是否更新完成
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// 更新完成日期
        /// </summary>
        public string DoneDate { get; set; }
    }
}
