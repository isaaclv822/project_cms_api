using project_cms.Models;

namespace project_cms.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user);
        User Update(User user);
        bool Delete(int id);
    }
}
