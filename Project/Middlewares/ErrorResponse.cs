namespace Project.Middlewares
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;
        public string? Code { get; set; }
        public string? TraceId { get; set; }
    }

}
