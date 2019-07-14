using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XamarinAadAuth.Backend.Models;

namespace XamarinAadAuth.Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get()
        {
            return new TodoItem
            {
                Id=111,
                Name="Item1",
                IsComplete = true
            };
        }
    }
}