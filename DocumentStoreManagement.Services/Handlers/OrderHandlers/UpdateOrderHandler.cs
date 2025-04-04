using DocumentStoreManagement.Core;
using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.OrderCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.OrderHandlers
{
    /// <inheritdoc/>
    public class UpdateOrderHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to update order
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Order>().UpdateAsync(command.Order);
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.RefreshMaterializedViewAsync(CustomConstants.MaterializedViewOrdersInclude);
        }
    }
}
