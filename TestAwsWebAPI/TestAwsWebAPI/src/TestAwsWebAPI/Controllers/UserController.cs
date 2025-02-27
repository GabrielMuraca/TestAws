﻿using Microsoft.AspNetCore.Mvc;

namespace TestAwsWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // GET api/users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "user1", "user2" };
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "user";
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody] string user)
        {
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string user)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
