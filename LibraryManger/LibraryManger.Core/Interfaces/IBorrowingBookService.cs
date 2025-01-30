using LibraryManger.Models.Data;
using LibraryManger.Models.Requests;

namespace LibraryManger.Core.Interfaces
{
    public interface IBorrowingBookService
    {
        public Task<SingleResult<Borrowing>> BorrowBook(BookBorrowingRequest request);
        public Task<SingleResult<Borrowing>> ReturnBook(BookReturningRequest request);
    }
}
