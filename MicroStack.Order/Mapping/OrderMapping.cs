using AutoMapper;
using EventBusRabbitMQ.Events;
using Order.Application.Commands.OrderCreate;

namespace MicroStack.Order.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap();
        }
    }
}
