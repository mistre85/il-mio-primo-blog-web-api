using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Models;
using NetCore.Data;
using Microsoft.EntityFrameworkCore;

namespace NetCore.Controllers.Api
{
    //api/post
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        BlogContext _ctx;

        public PostController()
        {
            _ctx = new BlogContext();
        }

        
        //api/post/get/[qualunque stringa]
        [HttpGet]
        public IActionResult Get(string? title)
        {
            IQueryable<Post> posts;

            if(title != null){
                posts = _ctx.Posts.Where(post => post.Title.ToLower().Contains(title.ToLower()));
            }
            else
            {
                posts = _ctx.Posts;
            }

            return Ok(posts.ToList<Post>());
        }

       
        //api/post/get/[qualqune numero]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Post post = _ctx.Posts.Where(p => p.Id == id).FirstOrDefault();

            return Ok(post);
        }

       
    }
}
