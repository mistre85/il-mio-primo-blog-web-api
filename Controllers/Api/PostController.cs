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


        //url query param
        //il parametro quindi dovrà essere
        //api/post/get/?title=[qualunque stringa]
        //api/post/get --> title è null e quindi ... (vedere codice sotto).
        [HttpGet]
        public IActionResult Get(string? title)
        {
            IQueryable<Post> posts;

            if (title != null)
            {
                posts = _ctx.Posts.Where(post => post.Title.ToLower().Contains(title.ToLower()));
            }
            else
            {
                posts = _ctx.Posts;
            }

            return Ok(posts.ToList<Post>());
        }

        //url param devono combaciare i nomi di {nomeparametro} con i parametri della action
        //e non ha nulla a che fare con il porgram
        //api/post/get/[qualqune numero]
        [HttpGet("{identification}")]
        public IActionResult Get(int identification)
        {
            Post post = _ctx.Posts.Where(p => p.Id == identification).FirstOrDefault();

            return Ok(post);
        }


    }
}
