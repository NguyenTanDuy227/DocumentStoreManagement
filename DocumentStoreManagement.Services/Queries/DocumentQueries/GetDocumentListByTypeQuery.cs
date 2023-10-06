﻿using DocumentStoreManagement.Core.Models;
using MediatR;

namespace DocumentStoreManagement.Services.Queries.DocumentQueries
{
    /// <summary>
    /// Query class to get documents by type
    /// </summary>
    public record GetDocumentListByTypeQuery(int Type) : IRequest<IEnumerable<Document>>;
}
