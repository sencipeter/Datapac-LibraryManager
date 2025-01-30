using LibraryManger.Core.Interfaces;
using LibraryManger.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibraryManger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            if (bookService is null)
                throw new ArgumentNullException(nameof(bookService));

            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            try
            {
                var book = (await _bookService.GetBookAsync(id)).GetResultOrThrowException();
                if (book == null)
                {
                    return NotFound();
                }
                return book;
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Internal server error!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    book = (await _bookService.UpdateBookAsync(book)).GetResultOrThrowException();
                    if (book == null)
                    {
                        return NotFound();
                    }
                    return book;
                }
                catch (ApplicationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception)
                {
                    return BadRequest("Internal server error!");
                }
            }
            else
            {
                var errors = ModelState
                        .Where(ms => ms.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );                
                return BadRequest(JsonSerializer.Serialize(errors));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBookById(int id)
        {
            try
            {
                var book = (await _bookService.DeleteBookAsync(id)).GetResultOrThrowException();
                if (book == null)
                {
                    return NotFound();
                }
                return book;
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Internal server error!");
            }
        }
    }
}
