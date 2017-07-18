using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace Ormus.EntityFramework.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly OrmusContext _context;

        public EFUserRepository(OrmusContext context)
        {
            _context = context;
            _context.Database.Log = (msg) => Debug.WriteLine(msg);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public int Delete(int id)
        {
            _context.Entry(new User { Id = id }).State = EntityState.Deleted;
            return _context.SaveChanges();
        }

        public User Get(int id)
        {
            return _context.Users.AsNoTracking()
                .Include(y => y.Role)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking()
                .Include(x => x.Role)
                .ToList();
        }

        public int Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return _context.SaveChanges();
        }
    }
}
