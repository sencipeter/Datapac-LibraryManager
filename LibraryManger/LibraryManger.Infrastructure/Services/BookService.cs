using LibraryManger.Core;
using LibraryManger.Core.Interfaces;
using LibraryManger.Core.Interfaces.Data;
using LibraryManger.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LibraryManger.Infrastructure.Services
{
    public class BookService : IBookService
    {
        readonly  AppDbContext _dbContext;
        readonly IAppRepository<Book> _appRepository;

        public BookService(AppDbContext dbContext, IAppRepository<Book> appRepository)
        {
            _dbContext = dbContext;
            _appRepository = appRepository;
        }

        public async Task<SingleResult<Book>> AddBook(Book book)
        {
            var result = new SingleResult<Book>();
            try
            {
                book = await _appRepository.AddAsync(book);
            }
            catch (Exception ex)
            {                
                result.Exception = ex;
            }
            return result;
        }

        public async Task<SingleResult<Book>> DeleteBook(int bookId)
        {
            var result = new SingleResult<Book>();
            try
            {
                result.Result = await _appRepository.GetByIdAsync(bookId);
                await _appRepository.DeleteAsync(new List<int> { bookId });
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public async Task<SingleResult<Book>> GetBook(int bookId, bool withAllBorrowings = false)
        {
            var result = new SingleResult<Book>();
            try
            {
                if (withAllBorrowings)
                {
                    result.Result = await _dbContext.Books
                        .Include(i => i.Borrowings)
                        .ThenInclude(i => i.User)
                        .FirstOrDefaultAsync(c =>c.Id == bookId);
                }
                else
                    result.Result = await _appRepository.GetByIdAsync(bookId);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public async Task<ListResult<Book>> ListBooks(Pagination? pagination, Sorting<Book>? sorting)
        {
            return await _appRepository.ListAsync(pagination, sorting); 
        }

        public async Task<SingleResult<Book>> UpdateBook(Book book)
        {
            var result = new SingleResult<Book>();
            try
            {

                result.Result = await _appRepository.GetByIdAsync(book.Id);                
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }
    }
}
