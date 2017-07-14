using Ormus.Core.Domain;
using System.Collections.Generic;

namespace Ormus.Core.Repositories
{
    public interface IUserRepository
    {
        User Get(int id);

        IEnumerable<User> GetAll();

        void Add(User user);

        int Update(User user);

        int Delete(int id);
    }
}
