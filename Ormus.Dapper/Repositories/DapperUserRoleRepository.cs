using Dapper;
using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Ormus.Dapper.Repositories
{
    public class DapperUserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public DapperUserRoleRepository(string connectionString) : base(connectionString)
        { }

        public void Add(UserRole userRole)
        {
            string query = @"INSERT INTO UsersRoles (Code, Description, CreatedDate, Ghost) VALUES
                        (@Code, @Description, @CreatedDate, @Ghost);

                        SELECT SCOPE_IDENTITY();";

            CommandDefinition commandDefinition = new CommandDefinition(query,
                new
                {
                    Code = userRole.Code,
                    Description = userRole.Description,
                    CreatedDate = userRole.CreatedDate,
                    Ghost = userRole.Ghost
                });

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                userRole.Id = conn.ExecuteScalar<int>(commandDefinition);
            }
        }

        public int Delete(int id)
        {
            int rowsAffected = 0;
            string query = @"DELETE FROM UsersRoles
                                WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                rowsAffected = conn.Execute(query, new { Id = id });
            }

            return rowsAffected;
        }

        public UserRole Get(int id)
        {
            UserRole userRole = null;

            string query = @"SELECT ur.Id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM UsersRoles ur 
                            WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                userRole = conn.QueryFirstOrDefault<UserRole>(query, new { id = id });
            }

            return userRole;
        }

        public IEnumerable<UserRole> GetAll()
        {
            IEnumerable<UserRole> userRoles;

            string query = @"SELECT ur.Id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM UsersRoles ur;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                userRoles = conn.Query<UserRole>(query);
            }

            return userRoles;
        }

        public int Update(UserRole userRole)
        {
            int rowsAffected = 0;
            string query = @"UPDATE UsersRoles
                                SET Code = @Code, Description = @Description, Ghost = @Ghost, CreatedDate = @CreatedDate
                                WHERE Id = @Id;";


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                rowsAffected = conn.Execute(query, userRole);
            }

            return rowsAffected;
        }
    }
}
