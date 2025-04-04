using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Commands.DocumentCommands;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.DocumentHandlers
{
    /// <inheritdoc/>
    public class CreateDocumentHandler<T>(IUnitOfWork unitOfWork) : IRequestHandler<CreateDocumentCommand<T>, T> where T : BaseEntity
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handler to create new document
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        public async Task<T> Handle(CreateDocumentCommand<T> command, CancellationToken cancellationToken)
        {
            await _unitOfWork.Repository<T>().AddAsync(command.Document, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return command.Document;
        }
    }
}
