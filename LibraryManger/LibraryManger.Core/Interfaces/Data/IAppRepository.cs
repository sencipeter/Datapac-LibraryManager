using LibraryManger.Models.Data;

namespace LibraryManger.Core.Interfaces.Data
{
    public interface IAppRepository<T>  where T : BaseModel
    {
        Task<T> GetByIdAsync(int id);        
        Task<ListResult<T>> ListAsync(Pagination? pagination = null, Sorting<T>? sorting = null);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(List<int> ids);
    }
}
