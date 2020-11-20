
using ServiceCommission.Models;


namespace ServiceCommission.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByNameAndPassword(string login, string password);

        User GetByEmail(string email);
    }

}
