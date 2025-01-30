using LibraryManger.Core;
using LibraryManger.Core.Interfaces;
using LibraryManger.Models.Data;
using LibraryManger.Models.Requests;

namespace LibraryManger.Infrastructure.Services
{
    public class BorrowingBookService:IBorrowingBookService
    {
        readonly AppDbContext _dbContext;
        
        public BorrowingBookService(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<SingleResult<Borrowing>> BorrowBook(BookBorrowingRequest request)
        {
            var result = new SingleResult<Borrowing>();
            try
            {

            }
            catch (Exception ex) 
            {
                result.Exception = ex;
                //log exception
            }
            return result;
        }

        public Task<SingleResult<Borrowing>> ReturnBook(BookReturningRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
