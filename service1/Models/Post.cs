namespace service1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public virtual Topic Topic { get; set; }
    }
}