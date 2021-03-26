using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context; 
        public UsersController( DataContext context )
        {
            this.context = context;
        }

        [HttpGet("{id}")]
                public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return this.context.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return this.context.Users.ToList();
        }
    }
}