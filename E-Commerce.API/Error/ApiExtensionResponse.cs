namespace E_Commerce.API.Error
{
   
        public class ApiExtensionResponse : ApiResponse
        {
            public string? Details { get; set; }
            public ApiExtensionResponse(int statusCode, string? Message = null, string? _Details = null) : base(statusCode, Message)
            {
                Details = _Details;
            }
        }
    

}
