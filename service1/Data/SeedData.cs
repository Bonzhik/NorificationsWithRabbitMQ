using service1.Models;
namespace service1.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "user1", Email = "user1@example.com" },
                    new User { Username = "user2", Email = "user2@example.com" }
                );
                context.SaveChanges();
            }

            if (!context.Topics.Any())
            {
                context.Topics.AddRange(
                    new Topic { Title = "Topic 1", Posts = new List<Post>(), Users = context.Users.ToList() },
                    new Topic { Title = "Topic 2", Posts = new List<Post>(), Users = context.Users.ToList() }
                );
                context.SaveChanges();
            }

            if (!context.Posts.Any())
            {
                var topics = context.Topics.ToList();
                context.Posts.AddRange(
                    new Post { Body = "Post 1 Body", Topic = topics.FirstOrDefault()},
                    new Post { Body = "Post 2 Body", Topic = topics.LastOrDefault()}
                );
                context.SaveChanges();
            }
        }
    }
}