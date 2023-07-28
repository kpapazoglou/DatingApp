

using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.ObjectPool;

namespace API.Controllers
{
    public class AccountController :BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }
    
 
//-----------------------------------------------------------------------------------------------------------------
        [HttpPost("Register")] //POST: api/account/register     )---> endpoint
      

        //sign in a new user
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)   //An async method runs synchronously until it reaches its first await expression, at which point the method is suspended until the awaited task is complete
        {

            if (await UserExist(registerDto.UserName)) return  BadRequest("Username exist"); //needs BAsiApiController to extend the ControllerBAse
            
            using var hmac = new HMACSHA512(); //class from dotnet ,a hash algorith,create a random genarated key , to salt the password 
            //using* in order to be able despose the class from memory

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //1: ComputeHash = method inside hmc class HMACSHA512 2: 
                PasswordSalt = hmac.Key //sa;t te password with the random ganarated key
            };

            _context.Users.Add(user); //track our new entity in memory 
            await _context.SaveChangesAsync(); //tell" database to  add our new user  !!await because we using async method

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }
//-------------------------------------------------------------------------------------------------------------------
        [HttpPost("login")]

        //login a user --->find the user on databse
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            
            if (user == null) return Unauthorized("invalid usenanme");

            //now to reverse tha hashing lines 39-40-41
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash  = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //compare each element of the byte array with the coresponting element of the other array, to find the user on database
            for (int i = 0 ; i < ComputeHash.Length; i ++)
            {
                if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");

            } 
 

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }


        private async Task<bool> UserExist(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());

        }

        
    }
}