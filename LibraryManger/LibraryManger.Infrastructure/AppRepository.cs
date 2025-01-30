using LibraryManger.Core;
using LibraryManger.Core.Interfaces.Data;
using LibraryManger.Infrastructure.Extensions;
using LibraryManger.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManger.Infrastructure
{
    public class AppRepository<T> : IAppRepository<T> where T : BaseModel
    {        
        protected readonly AppDbContext _appContext;

        public AppRepository(AppDbContext dbContext)
        {
            _appContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _appContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ListResult<T>> ListAsync(Pagination? pagination = null, Sorting<T>? sorting = null)
        {
            var query = _appContext.Set<T>().AsQueryable();

            if (sorting != null)
                query = new ProcessSorting<T>().Apply(sorting, query);

            return await query.PaginateAndExecuteAsync(pagination);
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.Created = entity.Updated = DateTime.Now;
            await _appContext.Set<T>().AddAsync(entity);
            await _appContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _appContext.Entry(entity).State = EntityState.Modified;
            entity.Updated = DateTime.Now;
            await _appContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(List<int> ids)
        {
            _appContext.Set<T>().RemoveRange(_appContext.Set<T>().Where(o => ids.Contains(o.Id)));
            await _appContext.SaveChangesAsync();
            return true;
        }
    }
}
