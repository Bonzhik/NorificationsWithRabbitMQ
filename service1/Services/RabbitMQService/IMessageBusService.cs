using service1.Dtos;
using service1.Models;

namespace service1.Services.RabbitMQService
{
    public interface IMessageBusService
    {
        public void PublishMessage(PostToMessage post);
    }
}