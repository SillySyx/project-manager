using ProjectManager.DTO;
using System.Collections.Generic;

namespace ProjectManager.Repositories
{
    public interface IProjectRepository : IDataSourceRepository<IProject>, IJsonFileRepository<List<IProject>>
    {
    }
}
