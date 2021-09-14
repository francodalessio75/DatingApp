namespace API.Errors
{
    public class ApiException
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        /**
        by setting parameters to null in case they are not provided they will be initialized to 
        null.
        */
        public ApiException(int statusCode, string message = null, string details = null)
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Details = details;
        }
    }
}