using service2.Models;

namespace service2.Services
{
    public class EventProcessor
    {
        private readonly EmailSender _emailSender;
        public EventProcessor(EmailSender emailSender)
        {
            _emailSender = emailSender;  
        }
        public void HandleEvent (Post post)
        {
            System.Console.WriteLine("HandleEvent");
            switch(post.Event)
            {
                case "Notify":
                    NotifySubs(post);
                    break;
            }
        }
        private void NotifySubs(Post post){
            System.Console.WriteLine("NotifySubs");
            var emails = post.Emails;
            var message = post.Body;
            foreach (var email in emails)
            {
                System.Console.WriteLine($"На адрес {email} будет выслано сообщение: {message}");
                _emailSender.SendEmail(email, message);
            }
        }
    }
}