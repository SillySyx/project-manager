using ProjectManager.DTO;
using System.Collections.Generic;

namespace ProjectManager.Repositories
{
    public interface IProjectActivityRepository : IDataSourceRepository<IProjectActivity>, IJsonFileRepository<List<IProjectActivity>>
    {
    }
}
