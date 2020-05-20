namespace ProjectManager.Repositories
{
    public interface IJsonFileRepository<T>
    {
        T Load(string filePath);

        void Save(string filePath, T data);
    }
}
