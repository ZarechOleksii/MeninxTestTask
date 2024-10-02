using DAL.Interfaces;
using Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebForms.Controllers
{
    public class BooksController : ApiController
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetBooksAsync()
        {
            return Ok(await _bookRepository.GetAllNoTrackingAsync());
        }

        /*
        [HttpGet]
        public async Task<IHttpActionResult> GetBooksAsync()
        {
            return Ok(await _bookRepository.GetAllAsync(5, 0, "", System.Data.SqlClient.SortOrder.Descending));
        }
        */
        [HttpGet]
        public async Task<IHttpActionResult> GetBook([FromUri] Guid id)
        {
            Book result = await _bookRepository.GetOneAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateBookAsync([FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                if (!_bookRepository.IsISBNAvailable(book.ISBN))
                {
                    ModelState.AddModelError($"{nameof(book)}.{nameof(book.ISBN)}", "ISBN already present");
                }

                if (book.Id != Guid.Empty && await _bookRepository.BookExists(book.Id))
                {
                    ModelState.AddModelError($"{nameof(book)}.{nameof(book.Id)}", "Id already present");
                }

                if (ModelState.IsValid)
                {
                    bool result = _bookRepository.Create(book);

                    if (result)
                    {
                        return Created(Url.Route("DefaultApi", new { id = book.Id }), book);
                    }

                    return BadRequest();
                }
            }

            return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
        }

        [HttpPut]
        public async Task<IHttpActionResult> EditBook([FromUri] Guid id, [FromBody] Book book)
        {
            if (!id.Equals(book.Id))
            {
                ModelState.AddModelError($"{nameof(book)}.{nameof(book.Id)}", "IDs should match");
            }

            if (ModelState.IsValid)
            {
                bool exists = await _bookRepository.BookExists(id);

                if (!exists)
                {
                    return NotFound();
                }

                bool result = _bookRepository.UpdateUntracked(book);

                if (result)
                {
                    return Ok();
                }

                return BadRequest();
            }

            return new ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBook([FromUri] Guid id)
        {
            Book dbBook = await _bookRepository.GetOneAsync(id);

            if (dbBook == null)
            {
                return NotFound();
            }

            bool result = _bookRepository.Delete(dbBook);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}