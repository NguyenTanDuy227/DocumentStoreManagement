using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Queries.OrderQueries;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    /// <inheritdoc/>
    public class GetOrderByIdHandler(IQueryRepository<Order> orderRepository) : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IQueryRepository<Order> _orderRepository = orderRepository;

        /// <summary>
        /// Handler to find order by id
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        public async Task<Order> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetByIdAsync(query.Id);
        }
    }
}
