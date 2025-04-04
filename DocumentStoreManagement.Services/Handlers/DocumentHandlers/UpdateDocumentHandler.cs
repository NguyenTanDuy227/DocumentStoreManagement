using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.DocumentCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.DocumentHandlers
{
    /// <inheritdoc/>
    public class UpdateDocumentHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateDocumentCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to update document
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(UpdateDocumentCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<Document>().UpdateAsync(command.Document);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
