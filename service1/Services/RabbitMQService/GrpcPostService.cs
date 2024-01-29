using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using service1.Data;
using service1.Dtos;

namespace service1.Services
{
    public class GrpcPostService : GrpcPost.GrpcPostBase
    {
        private readonly AppDbContext _context;

        public GrpcPostService(AppDbContext context)
        {
            _context = context;
        }

        public override Task<PostsResponse> GetPosts(GetPostsRequest request, ServerCallContext context)
        {
            var response = new PostsResponse();
            var posts = _context.Posts.Include(p => p.Topic).ToList();

            foreach (var post in posts) 
            {
                var postMessage = new GrpcPostModel{
                  PostId = post.Id,
                  Body = post.Body,
                  TopicId = post.Topic.Id  
                };
                response.Post.Add(postMessage);
            }
            return Task.FromResult(response);
        }
    }
    
}