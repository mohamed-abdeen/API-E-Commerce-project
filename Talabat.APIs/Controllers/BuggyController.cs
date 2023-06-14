using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
 
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet("Not found")]  // buggy/notfound
        public ActionResult GetNotFoundReqest()
        {
            var product = _context.products.Find(100);
            if(product==null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("Server Error")] // buggy/ServerError
        public ActionResult GetServerError()
        {
            var product = _context.products.Find(100);
            var producttoreturn = product.ToString(); // Excption [Null refrence Exception]
            return Ok(producttoreturn);

        }
        [HttpGet("Badrequest")] // buggy/Badrequest
        public ActionResult Badrequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("Badrequest/{id}")] //buggy/Badrequest/10
        public ActionResult GetBadrequest(int id) // validation Error
        {
            return Ok();
        }




    }
}
