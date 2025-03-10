namespace E_Commerce.API.Error
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null!)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode)!;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => " You are not Authorized",
                404 => "Resource found, it was not",
                500 => "Server Error",
                200=> "successfully",
                _ => null
            };
        }
      
    }
  
}
