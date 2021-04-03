using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController( DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto registerDto )
        {
            //gives back http 400 error
            if( await UserExists( registerDto.Username))
                return BadRequest("Username already taken");

            // "using" keyword assures the automatic execution of dispose method
            using var hmac = new HMACSHA512();

            //creats he user and its credentials
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash( Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            //get track of the framework insert method
            this.context.Users.Add(user);
            //executes the insert
            await this.context.SaveChangesAsync();

            //returns username and Json Web Token
            return new UserDto
            {
                Username = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login( LoginDto loginDto)
        {
            //ask database for a user having the same username
            var user = await context.Users.SingleOrDefaultAsync( user => user.UserName == loginDto.Username );
            //gives 400 if the user doesn't exists
            if( user == null ) return Unauthorized( "Invalid username" );

            //builds the db user password 
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //gets the transmitted password
            var computeHash = hmac.ComputeHash( Encoding.UTF8.GetBytes(loginDto.Password));

            //compares each byte of the password byte array if they are not equals return Unauthorized Exception
            for( int i = 0; i < computeHash.Length; i++ )
            {
                if( computeHash[i] != user.PasswordHash[i] ) return Unauthorized("Invalid Password");
            }

            //i everithing goes fine return username and Json Web Token
            return new UserDto
            {
                Username = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };


        }

        private async Task<bool> UserExists( string username )
        {
            return await context.Users.AnyAsync( user => user.UserName == username.ToLower());
        }
    }
}