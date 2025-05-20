using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Order.Application.Commands.OrderCreate;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroStack.Order.Consumers
{
    public class EventBusOrderCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnect;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection persistentConnect, IMediator mediator, IMapper mapper)
        {
            _persistentConnect = persistentConnect;
            _mediator = mediator;
            _mapper = mapper;
        }

        public void Consume()
        {
            if (_persistentConnect != null)
            {
                _persistentConnect.TryConnect();
            }

            var channel = _persistentConnect.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            var consumerTag = String.Empty; // otomatik oluşturulması için boş bırakılabilir
            var noLocal = false; // genelde false kullanılır
            var exclusive = false; // başkaları da kuyruktan tüketebilsin
            IDictionary<string, object> arguments = null; // özel argümanlarınız yoksa null kalabilir

            channel.BasicConsume(
                queue: EventBusConstants.OrderCreateQueue,
                autoAck: true,
                consumerTag: consumerTag,
                noLocal: noLocal,
                exclusive: exclusive,
                arguments: arguments,
                consumer: consumer
            );

        }

        private async void ReceivedEvent(object? sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message);

            if (e.RoutingKey == EventBusConstants.OrderCreateQueue)
            {
                var command = _mapper.Map<OrderCreateCommand>(@event);
                command.CreatedAt = DateTime.Now;
                command.TotalPrice = @event.Quantity * @event.Price;
                command.UnitPrice = @event.Price;
                
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _persistentConnect.Dispose();
        }
    }
}
