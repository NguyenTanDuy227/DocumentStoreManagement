using DocumentStoreManagement.Core;
using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.OrderCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    /// <inheritdoc/>
    public class CreateOrderHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to create order
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Order>().AddAsync(command.Order, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.RefreshMaterializedViewAsync(CustomConstants.MaterializedViewOrdersInclude);
            return command.Order;
        }
    }
}
