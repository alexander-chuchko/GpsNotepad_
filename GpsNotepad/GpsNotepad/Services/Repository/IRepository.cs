using GpsNotepad.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> DeleteAsync<T>(T entitty) where T : IEntityBase, new();
        Task<IEnumerable<T>> GetAllAsync<T>() where T : IEntityBase, new();
    }
}
