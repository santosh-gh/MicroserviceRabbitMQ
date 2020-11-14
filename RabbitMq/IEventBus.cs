namespace RabbitMq
{
    public interface IEventBus
    {
         void Publish(string message);
         void Subscribe();
    }
}