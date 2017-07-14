using Ormus.Core.Domain;
using System.Collections.Generic;

namespace Ormus.Core.Repositories
{
    public interface IUserRoleRepository
    {
        void Add(UserRole userRole);

        IEnumerable<UserRole> GetAll();

        UserRole Get(int id);

        int Update(UserRole userRole);

        int Delete(int id);
    }
}
