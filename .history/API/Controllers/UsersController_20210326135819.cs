using API.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        provate readonly DataContext 
        public UsersController( DataContext context )
        {
        }
    }
}