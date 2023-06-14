using System;

namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)

        {
            return statusCode switch
            {
                400 => "A Bad Request",
                401 => "Authorized, you are not",
                404 => "Resource is not found",
                500 => "Error are the path to the dark side , Error lead to anger , anger lead to hate , hate lead to carrer change ",
                _ => null
            };
        }


    }
}
