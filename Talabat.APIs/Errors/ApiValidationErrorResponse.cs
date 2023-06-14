using System.Collections.Generic;

namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse:ApiResponse
    {
        public IEnumerable<string > Error { get; set; }
        public ApiValidationErrorResponse():base(400)
        {

        }
    }
}
