namespace onilne_service.DTOs
{
    public class ResponseData<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
    public class ResponseStatus
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
    }
}
