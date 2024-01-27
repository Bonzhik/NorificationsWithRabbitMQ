using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using service1.Data;
using service1.Dtos;
using service1.Models;
using service1.Services.RabbitMQService;

namespace service1.Controllers
{
    [ApiController]
    [Route("api/pub/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMessageBusService _messageBus;

        public PostsController(AppDbContext context, IMessageBusService messageBus)
        {
            _context = context;
            _messageBus = messageBus;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost (PostDto postDto)
        {
            var post = new Post()
            {
                Id = postDto.Id,
                Body = postDto.Body,
                Topic = await  _context.Topics.FirstOrDefaultAsync(t => t.Id == postDto.topicId),
            };
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
            var topic = await _context.Topics.Include(t=>t.Users).FirstOrDefaultAsync(t => t.Id == postDto.topicId);
            var emails = topic.Users.Select(u => u.Email).ToArray();
            var postMessage = new PostToMessage()
            {
                Emails = emails,
                Body = postDto.Body,
                Event = "Notify"
            };
            try
            {
                _messageBus.PublishMessage(postMessage);
                System.Console.WriteLine("Message Is Published");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Произошла ошибка ----- > {ex.Message}");
            }
            return Ok();
        }
    }
}