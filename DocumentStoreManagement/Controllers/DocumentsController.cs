﻿using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Helpers;
using DocumentStoreManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentStoreManagement.Controllers
{
    /// <summary>
    /// Document Management API Controller - MongoDB database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly RedisCacheHelper _redisCacheHelper;
        private static readonly string cacheKey = "document-list-cache";

        /// <summary>
        /// Add dependencies to controller
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="documentService"></param>
        /// <param name="redisCacheHelper"></param>
        public DocumentsController(IUnitOfWork unitOfWork, IDocumentService documentService, RedisCacheHelper redisCacheHelper) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _redisCacheHelper = redisCacheHelper;
        }

        /// <summary>
        /// Gets the document list
        /// </summary>
        /// <returns>A list of all documents</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/documents
        ///
        /// </remarks>
        [HttpGet]
        public async Task<IEnumerable<Document>> GetDocuments()
        {
            // Set the expiration of cache
            TimeSpan expiration = TimeSpan.FromSeconds(30);

            // Get list of documents
            return await _redisCacheHelper.GetOrSetAsync(cacheKey, _documentService.GetAll, expiration);
        }

        /// <summary>
        /// Searches the document list by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A document list filtered by type</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/documents/{type}
        ///     
        /// ***NOTES***: To get data by type, enters one of the following values:
        /// * **1**: Gets the book data.
        /// * **2**: Gets the magazine data.
        /// * **3**: Gets the newspaper data.
        ///
        /// </remarks>
        [HttpGet("filter/{type}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsByType(int type)
        {
            try
            {
                // Get documents by type
                IEnumerable<Document> result = await _documentService.GetByType(type);
                return Ok(result);
            }
            catch (Exception e)
            {
                // Return error message
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Gets a document bases on document id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A document matches input id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/documents/{id}
        ///
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(string id)
        {
            // Get document by id
            Document document = await _documentService.GetById(id);
            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        /// <summary>
        /// Updates a document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedDocument"></param>
        /// <returns>An updated document</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/documents/{id}
        ///     {
        ///         "id": "id",
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "book": {
        ///           "authorName": "J. K. Rowling",
        ///           "pageNumber": 218
        ///         },
        ///         "magazine": {
        ///           "releaseNumber": 200,
        ///           "releaseMonth": "07/2023"
        ///         },
        ///         "newspaper": {
        ///           "releaseDate": "2023-10-04T08:44:20.351Z"
        ///         }
        ///     }
        ///
        /// ***NOTES***: Either book, magazine or newspaper is updated, the others will have null values.
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(string id, [FromBody] Document updatedDocument)
        {
            // Return bad request if ids don't match
            if (id != updatedDocument.Id)
            {
                return BadRequest();
            }

            try
            {
                // Update document
                await _documentService.Update(updatedDocument);
                await _unitOfWork.SaveAsync();

                // Clear cache
                await _redisCacheHelper.FlushAsync(cacheKey);
            }
            catch (Exception e)
            {
                // Check if document exists
                Document document = await _documentService.GetById(id);
                if (document == null)
                {
                    // Return document not found error
                    return NotFound();
                }
                else
                {
                    // Return error message
                    return BadRequest(e.Message);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a book
        /// </summary>
        /// <param name="newBook"></param>
        /// <returns>A newly created book</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/documents/books
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "authorName": "J. K. Rowling",
        ///         "pageNumber": 218
        ///     }
        ///
        /// </remarks>
        [HttpPost("books")]
        public async Task<ActionResult> PostBook([FromBody] Book newBook)
        {
            return await CreateDocument(newBook);
        }

        /// <summary>
        /// Creates a magazine
        /// </summary>
        /// <param name="newMagazine"></param>
        /// <returns>A newly created magazine</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/documents/magazines
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "releaseNumber": 200,
        ///         "releaseMonth": "07/2023"
        ///     }
        ///
        /// </remarks>
        [HttpPost("magazines")]
        public async Task<ActionResult> PostMagazine([FromBody] Magazine newMagazine)
        {
            return await CreateDocument(newMagazine);
        }

        /// <summary>
        /// Creates a newspaper
        /// </summary>
        /// <param name="newNewspaper"></param>
        /// <returns>A newly created newspaper</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/documents/newspapers
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "releaseDate": "2023-10-04T08:44:20.351Z"
        ///     }
        ///
        /// </remarks>
        [HttpPost("newspapers")]
        public async Task<ActionResult> PostNewspaper([FromBody] Newspaper newNewspaper)
        {
            return await CreateDocument(newNewspaper);
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Document is deleted</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/documents/{id}
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            // Get document by id
            Document document = await _documentService.GetById(id);
            if (document == null)
            {
                return NotFound();
            }

            // Delete document
            await _documentService.Delete(id);
            await _unitOfWork.SaveAsync();

            // Clear cache
            await _redisCacheHelper.FlushAsync(cacheKey);

            return NoContent();
        }

        #region Helpers 
        private async Task<ActionResult> CreateDocument(Document newDocument)
        {
            try
            {
                // Add a new document
                await _documentService.Create(newDocument);
                await _unitOfWork.SaveAsync();

                // Clear cache
                await _redisCacheHelper.FlushAsync(cacheKey);
            }
            catch (Exception e)
            {
                // Check if document exists
                Document document = await _documentService.GetById(newDocument.Id);
                if (document != null)
                {
                    // Return document already exists error
                    return Conflict();
                }
                else
                {
                    // Return error message
                    return BadRequest(e.Message);
                }
            }
            return CreatedAtAction(nameof(GetDocument), new { id = newDocument.Id }, newDocument);
        }
        #endregion
    }
}
