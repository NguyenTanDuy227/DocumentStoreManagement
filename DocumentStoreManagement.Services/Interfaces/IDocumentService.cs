﻿using DocumentStoreManagement.Core.Models.MongoDB;

namespace DocumentStoreManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for document service
    /// </summary>
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetAll();
        Task<IEnumerable<Document>> GetByType(string type);
        Task<Document> GetById(string id);
        Task Create(Document document);
        Task Update(Document document);
        Task Delete(string id);
    }
}
