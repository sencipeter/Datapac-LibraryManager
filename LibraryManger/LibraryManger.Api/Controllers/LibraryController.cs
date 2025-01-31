using LibraryManger.Core.Interfaces;
using LibraryManger.Models.Data;
using LibraryManger.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibraryManger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : Controller
    {
        private readonly IBorrowingBookService _borrowingBookService;
        public LibraryController(IBorrowingBookService borrowingBookService)
        {
            if (borrowingBookService is null)
                throw new ArgumentNullException(nameof(borrowingBookService));

            _borrowingBookService = borrowingBookService;
        }

        [HttpPost("borrowbook")]
        public async Task<ActionResult<Borrowing>> BorrowBook(BookBorrowingRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return (await _borrowingBookService.BorrowBook(request)).GetResultOrThrowException();

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

        [HttpPost("returnbook")]
        public async Task<ActionResult<Borrowing>> ReturnBook(BookReturningRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return (await _borrowingBookService.ReturnBook(request)).GetResultOrThrowException();

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
    }
}
