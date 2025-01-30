using LibraryManger.Models.Data;

namespace LibraryManger.Core.Interfaces
{
    public interface IBookService
    {
        public Task<SingleResult<Book>> AddBook(Book book);
        public Task<SingleResult<Book>> UpdateBook(Book book);
        public Task<SingleResult<Book>> DeleteBook(int bookId);
        public Task<SingleResult<Book>> GetBook(int bookId, bool withAllBorrowings = false);
        public Task<ListResult<Book>> ListBooks(Pagination? pagination, Sorting<Book>? sorting);
    }
}
