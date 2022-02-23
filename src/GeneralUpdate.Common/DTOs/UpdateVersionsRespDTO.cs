using System.Collections.Generic;

namespace GeneralUpdate.Common.DTOs
{
    public class UpdateVersionsRespDTO : BaseResponseDTO<UpdateVersionsDTO>
    { }

    public class UpdateVersionsDTO
    {
        public List<UpdateVersionDTO> UpdateVersions { get; set; }
    }
}