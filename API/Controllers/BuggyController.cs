using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            /**
              searches for something that for sure doesn't exists
              ( user having id = -1 )
              in order to have not found
            */
            var thing = _context.Users.Find(-1);
        
            if( thing == null ){
                return NotFound();
            }

            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetSrverError()
        {

            var thing = _context.Users.Find(-1);
            /**
                Generates an exception
            **/
            var thingToReturn = thing.ToString();

            return thingToReturn;   
            
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("This was not a good request!");
        }
    }

}