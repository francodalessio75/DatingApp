using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                //this configuration is needed because the property photoUrl is not passed through the API
                //first parameter og forMembers is wich property we want act on
                //the scond parameter is want we want to di on it, so we are saying thake the first having IsMain = true
                //and pass the propertu Url 
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom( src => 
                    src.Photos.FirstOrDefault( x => x.IsMain ).Url))
                    //this method was initially executed in AppUser entity but it caused problems of query optimization 
                    //missing on automapping from AppUser to MemberDto in UserRepository
                .ForMember(dest => dest.Age, opt => opt.MapFrom( src => src.DateOfBirth.CalculateAge()) );
            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();
        }
    }
}