
using System.Collections.Generic;

namespace GeneralUpdate.ClientCore.DTOs
{
    public class UpdateValidateRespDTO : BaseResponseDTO<UpdateValidateDTO>
    {
    }

    public class UpdateValidateDTO 
    {
        /// <summary>
        /// Is forcibly update.
        /// </summary>
        public bool IsForcibly { get; set; }

        /// <summary>
        /// 1:ClientApp 2:UpdateApp
        /// </summary>
        public int ClientType { get; set; }

        public List<UpdateVersionDTO> UpdateVersions { get; set; }
    }
}
