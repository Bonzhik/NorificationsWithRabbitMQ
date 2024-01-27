

namespace service1.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title {get; set;}
        public virtual ICollection<Post> Posts {get; set;}
        public virtual ICollection<User> Users{get; set;}

    }
}