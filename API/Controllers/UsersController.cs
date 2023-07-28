using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Authorize] //[AllowAnonymous] bypasses authorization statements. If you combine [AllowAnonymous] and an [Authorize] attribute, the [Authorize] attributes are ignored. For example if you apply [AllowAnonymous] at the controller level:
                    //Any authorization requirements from [Authorize] attributes on the same controller or action methods on the controller are ignored.
                    //Authentication middleware is not short-circuited but doesn't need to succeed.
    public class UsersController :BaseApiController
    {
      


        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {

            var users = await _context.Users.ToListAsync();

            return users;
        }


    
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);

        }

    }
}