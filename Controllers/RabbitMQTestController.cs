using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace rabbitmq_sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQTestController : ControllerBase
    {
        private readonly ILogger<RabbitMQTestController> _logger;

        public RabbitMQTestController(ILogger<RabbitMQTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "SendHelloMessage")]
        public String SendHelloMessage()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq-swift-ft-dev.apps.ddc-test.corp.intranet" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
            }
            return "Success";
        }

    }
}