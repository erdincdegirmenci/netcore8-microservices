using AutoMapper;
using EventBusRabbitMQ.Events;
using MicroStack.Sourcing.Entitites;

namespace MicroStack.Sourcing.Mapping
{
    public class SourcingMapping : Profile
    {
        public SourcingMapping() 
        {

            CreateMap<OrderCreateEvent, Bid>().ReverseMap();
        }
    }
}
