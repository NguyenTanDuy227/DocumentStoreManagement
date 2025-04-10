using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentStoreManagement.Controllers
{
    /// <summary>
    /// Document Management API Controller
    /// </summary>
    /// <remarks>
    /// Add dependencies to controller
    /// </remarks>
    /// <param name="documentService"></param>
    ///// <param name="cacheService"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController(IDocumentService documentService/*, ICacheService cacheService*/) : BaseController
    {
        private readonly IDocumentService _documentService = documentService;
        //private readonly ICacheService _cacheService = cacheService;
        private static readonly string cacheKey = "document-list-cache";

        /// <summary>
        /// Searches the document list by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A document list filtered by type</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/documents/filter/{type}
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
                // Set the expiration of cache
                TimeSpan expiration = TimeSpan.FromMinutes(30);

                // Get list of documents
                //return Ok(await _cacheService.GetOrSetAsync(
                //    key: $"{cacheKey}-{type}",
                //    func: () => _documentService.GetByType(type),
                //    expiration: expiration));
                return Ok(await _documentService.GetByType(type));
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
            Document document;
            try
            {
                // Try to find a document
                document = await _documentService.GetById(id);
            }
            catch (Exception e)
            {
                // Return not found error message
                return NotFound(e.Message);
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
        ///         "id": "Id",
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(string id, Document updatedDocument)
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
            }
            catch (Exception e)
            {
                // Check if document exists
                if (!await DocumentExists(id))
                {
                    // Return document not found error
                    return NotFound();
                }

                // Return error message
                return BadRequest(e.Message);
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
        ///     POST api/documents/book
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "authorName": "J. K. Rowling",
        ///         "pageNumber": 218
        ///     }
        ///
        /// </remarks>
        [HttpPost("book")]
        public async Task<ActionResult> PostBook(Book newBook)
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
        ///     POST api/documents/magazine
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "releaseNumber": 200,
        ///         "releaseMonth": "07/2023"
        ///     }
        ///
        /// </remarks>
        [HttpPost("magazine")]
        public async Task<ActionResult> PostMagazine(Magazine newMagazine)
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
        ///     POST api/documents/newspaper
        ///     {
        ///         "publisherName": "Example Name",
        ///         "releaseQuantity": 12,
        ///         "releaseDate": "2023-10-04T08:44:20.351Z"
        ///     }
        ///
        /// </remarks>
        [HttpPost("newspaper")]
        public async Task<ActionResult> PostNewspaper(Newspaper newNewspaper)
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
            await _documentService.Delete(document);

            return NoContent();
        }

        #region Helpers
        /// <summary>
        /// Generic method to create new document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newDocument"></param>
        private async Task<ActionResult> CreateDocument<T>(T newDocument) where T : BaseEntity
        {
            try
            {
                // Add a new document
                await _documentService.Create(newDocument);
            }
            catch (Exception e)
            {
                // Check if document exists
                if (await DocumentExists(newDocument.Id))
                {
                    // Return document already exists error
                    return Conflict();
                }

                // Return error message
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(GetDocument), new { id = newDocument.Id }, newDocument);
        }

        /// <summary>
        /// Check if document exists method
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean</returns>
        private async Task<bool> DocumentExists(string id)
        {
            return await _documentService.GetById(id) != null;
        }
        #endregion
    }
}
