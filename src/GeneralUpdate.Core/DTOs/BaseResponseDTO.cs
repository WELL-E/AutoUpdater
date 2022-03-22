namespace GeneralUpdate.Core.DTOs
{
    public class BaseResponseDTO<T> : IResponse
    {
        public int Code { get; set; }

        public T Body { get; set; }

        public string Message { get; set; }
    }
}