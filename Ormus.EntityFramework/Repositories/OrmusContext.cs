using Ormus.Core.Domain;
using System.Data.Entity;

namespace Ormus.EntityFramework.Repositories
{
    public class OrmusContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        public OrmusContext(string cs) : base (cs)
        {
            Database.SetInitializer<OrmusContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // specifying UserRole table name
            var itemBuilder = modelBuilder.Entity<UserRole>();
            itemBuilder.ToTable("UsersRoles");

            // foreign key on User table <= not needed after adding RoleId property on User class
            //modelBuilder.Entity<User>()
            //    .HasRequired(x => x.Role)
            //    .WithOptional()
            //    .Map(r => r.MapKey("RoleId"));
        }
    }
}
