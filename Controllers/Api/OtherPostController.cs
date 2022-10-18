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
    public class OtherPostController : ControllerBase
    {

        BlogContext _ctx;

        public OtherPostController()
        {
            _ctx = new BlogContext();
        }


        //api/otherpost/getallpost/[qualunque stringa]
        [Route("get/all")]
        [HttpGet]
        public IActionResult GetAllPost(string? title)
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


        //api/otherpost/getpostdetail/[qualqune numero]
        [Route("get/detail")]
        [HttpGet("{id}")]
        public IActionResult GetPostDetail(int id)
        {
            Post post = _ctx.Posts.Where(p => p.Id == id).FirstOrDefault();

            return Ok(post);
        }

       
    }
}
