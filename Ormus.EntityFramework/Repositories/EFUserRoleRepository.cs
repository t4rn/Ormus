using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace Ormus.EntityFramework.Repositories
{
    public class EFUserRoleRepository : IUserRoleRepository
    {
        private readonly OrmusContext _context;

        public EFUserRoleRepository(OrmusContext context)
        {
            _context = context;
            _context.Database.Log = (msg) => Debug.WriteLine(msg);
        }

        public void Add(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }

        public int Delete(int id)
        {
            // additional sql query to get UserRole...
            //UserRole userRole = _context.UserRoles.Find(id);
            //_context.UserRoles.Remove(userRole);

            _context.Entry(new UserRole() { Id = id }).State = EntityState.Deleted;
            return _context.SaveChanges();
        }

        public UserRole Get(int id)
        {
            return _context.UserRoles
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserRole> GetAll()
        {
            return _context.UserRoles
                .AsNoTracking()
                .ToList();
        }

        public int Update(UserRole userRole)
        {
            _context.Entry(userRole).State = EntityState.Modified;
            return _context.SaveChanges();
        }
    }
}
