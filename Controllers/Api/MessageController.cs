using Microsoft.AspNetCore.Mvc;
using NetCore.Data;
using NetCore_01.Models;

namespace NetCore_01.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
       
        [HttpPost]
        public IActionResult Send([FromBody] Message message)
        {
           
            BlogContext ctx = new BlogContext();

            ctx.Messages.Add(message);
            ctx.SaveChanges();

            return Ok();
        }


    }

    

    //from body - differenze con altri dati e chiamate
    //validation: possiamo entrare nella action?
    //dettagli tra {} - [] nelle pattern matching delle rotte
}
