namespace GeneralUpdate.ClientCore.DTOs
{
    public class BaseRequestDTO<T> : IRequest
    {
        public int Code { get; set; }

        public T Body { get; set; }

        public string Message { get; set; }
    }
}
