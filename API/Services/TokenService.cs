using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key; // _i katw pavla  epidi private
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim> //the user claims hes name
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName) //we can have more that one , thats why we use list
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);   //encrypt the key with this algorithm

            //describe the tokken that we are going to return
            var tokkenDescriptor = new SecurityTokenDescriptor
            {//properties
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //set for 7 days , good 
                SigningCredentials = creds
            };

            var tokenHandler =  new JwtSecurityTokenHandler(); 

            var token = tokenHandler.CreateToken(tokkenDescriptor); //create our token , whichs include our claims etc

            return tokenHandler.WriteToken(token);
        }  

    }   
}