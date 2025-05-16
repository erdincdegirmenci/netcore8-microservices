using AutoMapper;
using Order.Application.Commands.OrderCreate;
using Order.Application.Responses;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Mapper
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderEntity, OrderCreateCommand>().ReverseMap();
            CreateMap<OrderEntity, OrderResponse>().ReverseMap();
        }
    }
}
