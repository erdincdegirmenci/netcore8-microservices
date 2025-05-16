using MediatR;
using Order.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Queries
{
    public class GetOrdersBySellerUsernameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string UserName { get; set; }

        public GetOrdersBySellerUsernameQuery(string userName)
        {
            UserName = userName;
        }
    }
}
