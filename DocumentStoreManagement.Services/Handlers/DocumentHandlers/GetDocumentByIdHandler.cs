using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Queries.DocumentQueries;
using MediatR;

namespace DocumentStoreManagement.Services.Handlers.DocumentHandlers
{
    /// <inheritdoc/>
    public class GetDocumentByIdHandler(IQueryRepository<Document> documentRepository) : IRequestHandler<GetDocumentByIdQuery, Document>
    {
        private readonly IQueryRepository<Document> _documentRepository = documentRepository;

        /// <summary>
        /// Handler to find document by id
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        public async Task<Document> Handle(GetDocumentByIdQuery query, CancellationToken cancellationToken)
        {
            return await _documentRepository.GetByIdAsync(query.Id);
        }
    }
}
