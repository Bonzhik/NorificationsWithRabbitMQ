using Grpc.Net.Client;
namespace service2.Services
{
    public class GrpcPostClientService
    {
        private readonly IConfiguration _configuration;
        public GrpcPostClientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<GrpcPostModel> ReturnAllPosts()
        {
            var channel = GrpcChannel.ForAddress(_configuration["Service1"]);
            var client = new GrpcPost.GrpcPostClient(channel);
            var request = new GetPostsRequest();

            try{
                var reply = client.GetPosts(request);
                return reply.Post;
            }catch(Exception ex)
            {
                System.Console.WriteLine($"Conn failed to gRpc ---> {ex.Message}");
                return null;
            }
        }
    }
}