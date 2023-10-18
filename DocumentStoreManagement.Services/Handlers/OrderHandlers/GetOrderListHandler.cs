﻿using DocumentStoreManagement.Core;
using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Queries.OrderQueries;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    public class GetOrderListHandler : IRequestHandler<GetOrderListQuery, IEnumerable<Order>>
    {
        private readonly IQueryRepository<Order> _orderRepository;

        public GetOrderListHandler(IQueryRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler to get all orders
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        public async Task<IEnumerable<Order>> Handle(GetOrderListQuery query, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetAllAsync(CustomConstants.OrdersTable);
        }
    }
}
