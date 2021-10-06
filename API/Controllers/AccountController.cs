using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //gives back http 400 error
            if (await UserExists(registerDto.Username))
                return BadRequest("Username already taken");

            var user = _mapper.Map<AppUser>(registerDto);

            // "using" keyword assures the automatic execution of dispose method
            //instantiates a HMACSHA512 object by a random key. This class will be used 
            //to compute the ashcode of the password
            using var hmac = new HMACSHA512();

            //creats he user and its credentials
            user.UserName = registerDto.Username.ToLower();
            //password ash code
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            //thw equals password will give back two different PasswordSalt
            user.PasswordSalt = hmac.Key;
            
            //get track of the framework insert method
            _context.Users.Add(user);
            //executes the insert
            await _context.SaveChangesAsync();

            //returns username and Json Web Token
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //searches in database a user by username value. If it finds more then one user having that username
            //it just fails
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(user => user.UserName == loginDto.Username);

            //gives 400 if the user doesn't exists
            if (user == null) return Unauthorized("Invalid username");

            //initialize a HMACSHA512 object by using the same key used to compute the user password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //computes the ash code of the password using the same key and compres it with the one stored in the database
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //compares each byte of the password byte array if they are not equals return Unauthorized Exception
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            //i everithing goes fine return username and Json Web Token
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };


        }

        private async Task<bool> UserExists(string username)
        {
            //executes the check on all database users
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}