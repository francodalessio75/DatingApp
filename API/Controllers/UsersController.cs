using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{

    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {

            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null )
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
                
            };

            if(user.Photos.Count == 0 )
            {
                photo.IsMain = true;
            }
            
            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDto>(photo);
                // return CreatedAtRoute("GetUser",_mapper.Map<PhotoDto>(photo));
                return CreatedAtRoute("GetUser",new {username = user.UserName},_mapper.Map<PhotoDto>(photo));
            }
            else
            {
                return BadRequest("Problem adding photo");
            }
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> setMainPhoto( int photoId )
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            
            var photo = user.Photos.FirstOrDefault( x => x.Id == photoId );

            if(photo.IsMain) return BadRequest("This is already your main photo.");

            var currentMain = user.Photos.FirstOrDefault( x => x.IsMain );
            if(currentMain != null)
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> deletePhoto(int PhotoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == PhotoId);

            if(photo == null ) return NotFound();

            if(photo.IsMain) return BadRequest("You are trying to delete the main photo!");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if( result.Error != null ) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo.");
        }
    }
}