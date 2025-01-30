using LibraryManger.Models.Data;

namespace LibraryManger.Core.Interfaces
{
    public interface IBookService
    {
        public Task<SingleResult<Book>> AddBookAsync(Book book);
        public Task<SingleResult<Book>> UpdateBookAsync(Book book);
        public Task<SingleResult<Book>> DeleteBookAsync(int bookId);
        public Task<SingleResult<Book>> GetBookAsync(int bookId, bool withAllBorrowings = false);
        public Task<ListResult<Book>> ListBooksAsync(Pagination? pagination, Sorting<Book>? sorting);
    }
}
