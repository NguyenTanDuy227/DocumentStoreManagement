using DocumentStoreManagement.Core;
using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.OrderCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    /// <inheritdoc/>
    public class DeleteOrderHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to delete order
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Order>().RemoveAsync(command.Order);
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.RefreshMaterializedViewAsync(CustomConstants.MaterializedViewOrdersInclude);
        }
    }
}
