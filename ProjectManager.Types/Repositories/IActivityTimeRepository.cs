using ProjectManager.DTO;
using System.Collections.Generic;

namespace ProjectManager.Repositories
{
    public interface IActivityTimeRepository : IDataSourceRepository<IActivityTime>, IJsonFileRepository<List<IActivityTime>>
    {
    }
}
