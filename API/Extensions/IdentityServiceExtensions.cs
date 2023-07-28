
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{   //static ---> we can use the methods inside without instantiating  a new instace of the class
    public static class IdentityServiceExtensions
    {
        public static  IServiceCollection AddIdentityServices (this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {//specify all the rules of how our server shopould authenticate
                        //our serve is gonna check for tokenssinginkEy and make sure is valid based upon the issue assing key we specify next
                        ValidateIssuerSigningKey = true,
                        //specify what our issue assign key is
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding
                            .UTF8.GetBytes(config["TokenKey"])),
                        //the issuer of our token is the API server , we need to pass that with the token , but we have already implement that
                        ValidateIssuer = false,
                        //we have not pass that info with our toke , so false
                        ValidateAudience = false
                    };
                });
            
            return services;
        }
    }
}