namespace service1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username {get;set;}
        public string Email {get; set;}
        public virtual ICollection<Topic> Topics {get; set;}
    }
}