using System.Collections.Generic;

namespace GeneralUpdate.Core.DTOs
{
    public class UpdateVersionsRespDTO : BaseResponseDTO<UpdateVersionsDTO>
    { }

    public class UpdateVersionsDTO
    {
        public List<UpdateVersionDTO> UpdateVersions { get; set; }
    }
}