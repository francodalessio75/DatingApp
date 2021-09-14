using System;
using System.Collections.Generic;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        //it must be necessarily be called that, 
        //if not it'll be not recognized by the framework        public int Id { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        /* Commented out since using it the query optimization on dataContext level
           obtained by mapping with automapper AppUser => MemberDto just fails and
           when the GetUser quesry is executed we also retrieve password and ashSalt.
           The age is now computed by executing the same extension method but on dataContext
           level in UserRepository methods*/
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
    }
}