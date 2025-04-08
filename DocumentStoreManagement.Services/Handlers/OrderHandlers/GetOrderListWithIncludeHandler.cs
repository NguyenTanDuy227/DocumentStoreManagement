using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Queries.OrderQueries;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    /// <inheritdoc/>
    public class GetOrderListWithIncludeHandler(IQueryRepository<Order> orderRepository) : IRequestHandler<GetOrderListWithIncludeQuery, IEnumerable<Order>>
    {
        private readonly IQueryRepository<Order> _orderRepository = orderRepository;

        /// <summary>
        /// Handler to get all orders with include
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Order>> Handle(GetOrderListWithIncludeQuery request, CancellationToken cancellationToken)
        {
            // Query from orders table join with orderDetails
            return await _orderRepository.GetAllAsync();
        }
    }
}
