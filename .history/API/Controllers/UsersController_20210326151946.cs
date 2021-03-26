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

        [HttpGet]
        public async ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return await this.context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async ActionResult<AppUser> GetUsers( int id )
        {
            return this.context.Users.Find(id);
        }
    }
}