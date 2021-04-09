using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob )
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            //If the bithday has not been yet celebrated during the current year, 
            //then the  age must be 
            
            if(dob.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }   
    }
    
}