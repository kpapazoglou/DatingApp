using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext __context;
        public BuggyController(DataContext context) //inject Data
        {
            __context = context;

        }

        /*  we use the authorized attributes on this because this one's purpose is simply to ensure

            that we can return a401 unauthorized from this when a user is not authenticating against this particular

            endpoint.*/
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {

            return "secret";
        }
        //And we're going to attempt to find a user that does not exist.
        //And it's simply not possible for us to have a user with an ID of minus one.


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {   
            var thing = __context.Users.Find(-1);
            if (thing == null) return NotFound(); //not found http response
            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerEroor()
        {
            var thing = __context.Users.Find(-1);

            var thingToReturn = thing.ToString(); //we cannot turn null into a string -->nullreference exception , that is the goal here

            return thingToReturn;
           
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
           
        }
    }
}