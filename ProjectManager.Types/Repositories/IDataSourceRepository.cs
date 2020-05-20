using System;
using System.Linq;

namespace ProjectManager.Repositories
{
    public interface IDataSourceRepository<T>
    {
        T Add(T entry);

        T Get(Guid id);

        IQueryable<T> GetAll();

        bool Update(T entry);

        bool Delete(T entry);
        bool Delete(Guid id);
    }
}
