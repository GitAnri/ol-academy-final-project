namespace Project.Business.Exceptions
{
    public abstract class ApiException : Exception
    {
        public string Code { get; }

        protected ApiException(string message, string code) : base(message)
        {
            Code = code;
        }
    }

    public class ValidationException : ApiException
    {
        public ValidationException(string message, string code = "VALIDATION_ERROR")
            : base(message, code) { }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException(string message, string code = "NOT_FOUND")
            : base(message, code) { }
    }

    public class ConflictException : ApiException
    {
        public ConflictException(string message, string code = "CONFLICT")
            : base(message, code) { }
    }

    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message, string code = "FORBIDDEN")
            : base(message, code) { }
    }
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message, string code = "UNAUTHORIZED")
            : base(message, code) { }

    }

}
