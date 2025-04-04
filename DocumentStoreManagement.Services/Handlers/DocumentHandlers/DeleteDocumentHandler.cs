using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.DocumentCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.DocumentHandlers
{
    /// <inheritdoc/>
    public class DeleteDocumentHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDocumentCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to delete document
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Document>().RemoveAsync(command.Document);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
