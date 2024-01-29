using Microsoft.AspNetCore.Mvc;
using service2.Services;

namespace service2.Controllers
{
    [ApiController]
    [Route("api/sub/[controller]")]
    public class PostController: ControllerBase
    {
        private readonly GrpcPostClientService _grpcPostClientService;

        public PostController(GrpcPostClientService grpcPostClientService)
        {
            _grpcPostClientService = grpcPostClientService;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            var posts = _grpcPostClientService.ReturnAllPosts();

            return Ok(posts);
        }
    }
}