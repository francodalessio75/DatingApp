using System;
using System.Collections.Generic;

namespace API.DTOs
{
    /*
    this class is necessary to avoid the circular reference between AppUser and Photo
    When we query fro users the framework executes a query also for photos that in turn
    have reference to AppUser.
    With this class we map the UserDto amd we figure out the problem 
    */
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
         public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }

    }
}