
namespace project_cms.Interfaces
{
    public interface IArticleRepository
    {
        IEnumerable<object> GetAll();
        object GetById(int id);
    }
}
