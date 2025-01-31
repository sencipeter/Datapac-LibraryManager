using LibraryManger.Core;
using LibraryManger.Core.Interfaces;
using LibraryManger.Core.Interfaces.Data;
using LibraryManger.Models.Data;
using LibraryManger.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace LibraryManger.Infrastructure.Services
{
    public class BorrowingBookService:IBorrowingBookService
    {
        readonly AppDbContext _dbContext;
        readonly IAppRepository<Borrowing> _borrowingRepository;
        readonly IAppRepository<Book> _bookRepository;

        public BorrowingBookService(AppDbContext dbContext,
            IAppRepository<Borrowing> borrowingRepository,
            IAppRepository<Book> bookRepository) 
        {
            _dbContext = dbContext;
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
        }

        public async Task<SingleResult<Borrowing>> BorrowBook(BookBorrowingRequest request)
        {
            var result = new SingleResult<Borrowing>();
            try
            {
                if (request is null)
                    throw new ArgumentNullException(nameof(request));

                if (request.UserId is null)
                    throw new NullReferenceException(nameof(request.UserId));

                if (request.BookId is null)
                    throw new NullReferenceException(nameof(request.BookId));


                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);
                if (book is null)
                    throw new ApplicationException($"Book \"{request.BookId}\" Not Founded!");
                
                var user = await _dbContext.Users.FirstOrDefaultAsync(b => b.Id == request.UserId);
                if (user is null)
                    throw new ApplicationException($"User \"{request.UserId}\" Not Founded!");

                if (book.IsBorrowed)
                    throw new ApplicationException($"Book \"{request.BookId}\" is already borrowed!");

                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    result.Result = await _borrowingRepository.AddAsync(new Borrowing
                    {
                        Active = true,
                        UserId = request.UserId.Value,
                        BookId = request.BookId.Value,
                        BorrowedAt = DateTime.Now
                    });

                    book.IsBorrowed = true;
                    await _bookRepository.UpdateAsync(book);
                    scope.Complete();
                }

                result.Result.User = user;
                result.Result.Book = book;
            }
            catch (Exception ex) 
            {
                result.Exception = ex;
                //log exception
            }
            return result;
        }

        public async Task<SingleResult<Borrowing>> ReturnBook(BookReturningRequest request)
        {
            var result = new SingleResult<Borrowing>();
            try
            {
                if (request is null)
                    throw new ArgumentNullException(nameof(request));                

                if (request.BookId is null)
                    throw new NullReferenceException(nameof(request.BookId));


                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == request.BookId);
                if (book is null)
                    throw new ApplicationException($"Book \"{request.BookId}\" Not Founded!");

                if (!book.IsBorrowed)
                    throw new ApplicationException($"Book \"{request.BookId}\" is already returned!");

                var actualBorrowing = await _dbContext.Borrowings.FirstOrDefaultAsync(b => b.BookId == request.BookId && b.ReturnedAt == null);                
                if (actualBorrowing is null)
                    throw new ApplicationException($"Book  \"{request.BookId}\" was not borrowed correctly!");
               
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    actualBorrowing.ReturnedAt = DateTime.Now;
                    await _borrowingRepository.UpdateAsync(actualBorrowing);

                    book.IsBorrowed = false;
                    await _bookRepository.UpdateAsync(book);
                    scope.Complete();
                }

                result.Result = actualBorrowing;
                result.Result.Book = book;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                //log exception
            }
            return result;
        }
    }
}
