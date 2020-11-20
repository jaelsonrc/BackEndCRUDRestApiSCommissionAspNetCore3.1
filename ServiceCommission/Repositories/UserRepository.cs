using ServiceCommission.Models;
using ServiceCommission.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceCommission.Utils;

namespace ServiceCommission.Repositories
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context):base(context)
        {                
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(f => f.Email == email);
        }

        public User GetByNameAndPassword(string login, string password)
        {
            return _context.Users.FirstOrDefault(f => f.Login == login && f.Password == password.GetHash(login));
        }
    }
}
