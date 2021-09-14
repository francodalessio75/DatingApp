
using System.Threading.Tasks;
using System.Text;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace DatingApp.API.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
   
   // Dependency Injection
   public AuthenticationMiddleware(RequestDelegate next)
   {
       _next = next;
   }

   public async Task InvokeAsync(HttpContext context)
   {
       try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {  
               ;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
               

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            
            }
	   //Reading the AuthHeader which is signed with JWT
       string authHeader = context.Request.Headers["Authorization"];

       if (authHeader != null)
       {   
	       //Reading the JWT middle part           
           int startPoint = authHeader.IndexOf(".") + 1;               
           int endPoint = authHeader.LastIndexOf(".");

           var tokenString = authHeader
	           .Substring(startPoint, endPoint - startPoint).Split(".");
           var token = tokenString[0].ToString()+"==";

           var credentialString = Encoding.UTF8
               .GetString(Convert.FromBase64String(token));
        
           // Splitting the data from Jwt
           var credentials = credentialString.Split(new char[] { ':',',' });

           // Trim this Username and UserRole.
           var userRule = credentials[5].Replace("\"", ""); 
           var userName = credentials[3].Replace("\"", "");

            // Identity Principal
           var claims = new[]
           {
               new Claim("name", userName),
               new Claim(ClaimTypes.Role, userRule),
           };
           var identity = new ClaimsIdentity(claims, "basic");
           context.User = new ClaimsPrincipal(identity);
       }
       //Pass to the next middleware
       await _next(context);
   } 
    }
}