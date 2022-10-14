using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Models;
using NetCore.Data;
using Microsoft.EntityFrameworkCore;

namespace NetCore.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        BlogContext _ctx;

        public PostController()
        {
            _ctx = new BlogContext();
        }
        public IActionResult Get()
        {

            List<Post> posts = _ctx.Posts.ToList();

            return NotFound(new { Message = "Oggetto non trovato"});
        }
    }
}
