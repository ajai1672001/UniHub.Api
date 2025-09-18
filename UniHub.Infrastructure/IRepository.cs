using System.Linq.Expressions;
using UniHub.Domain.Entities;

namespace UniHub.Infrastructure;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();

    // ✅ Get All Records with Filtering
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null, bool ignoreQueryFilter = false, bool hasNoTracking = false);

    // ✅ Get Single Record by ID
    Task<T> GetByIdAsync(int id);

    // ✅ Insert Single Record
    Task<T> InsertAsync(T entity);

    // ✅ Insert Bulk Records
    Task<IEnumerable<T>> BulkInsertAsync(IEnumerable<T> entities);

    // ✅ Update Single Record
    void Update(T entity);

    // ✅ Update Bulk Records
    void BulkUpdate(IEnumerable<T> entities);

    // ✅ Delete Single Record
    void Delete(T entity);

    // ✅ Delete Bulk Records
    void BulkDelete(IEnumerable<T> entities);
}