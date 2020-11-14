using System;
using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq
{
    public class RabbitMqBus : IEventBus
    {
        private readonly IHostApplicationLifetime _lifetime;
        public RabbitMqBus(){}
        public RabbitMqBus(IHostApplicationLifetime lifetime)
        {
                _lifetime = lifetime;
        }
         public void Publish(string message)
         {
                var factory = new ConnectionFactory{
                    HostName = "host.docker.internal",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672
                };
                
            using(var connection = factory.CreateConnection()){
                using(var channel = connection.CreateModel()){
                    channel.QueueDeclare("eshopq",false,false,false,null);
                    var byteMess = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("","eshopq",body:byteMess);
                    Console.WriteLine("Message Published{0}",message);
                }
            }

         }
         public void Subscribe()
         {
             _lifetime.ApplicationStarted.Register(()=>{
                 SubscribeProcess();
             });
         }

         public void SubscribeProcess()
         {
                 var factory = new ConnectionFactory{
                    HostName = "host.docker.internal",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672
                };
                
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueDeclare("eshopq",false,false,false,null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ((s,e) =>{
                    var body = e.Body.ToArray();
                    var mess = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Message Subscribed {0}",mess);
                });
                channel.BasicConsume("eshopq",autoAck:true,consumer);
         }
    }
}