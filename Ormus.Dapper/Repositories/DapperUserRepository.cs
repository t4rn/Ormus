using Dapper;
using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Ormus.Dapper.Repositories
{
    public class DapperUserRepository : BaseRepository, IUserRepository
    {
        public DapperUserRepository(string connectionString) : base(connectionString)
        { }

        public void Add(User user)
        {
            string query = @"INSERT INTO Users (RoleId, Login, Password, FirstName, 
                                    LastName, Email, CreatedDate, UpdatedDate, Ghost) VALUES
                        (@RoleId, @Login, @Password, @FirstName, 
                        @LastName, @Email, @CreatedDate, @UpdatedDate, @Ghost);

                        SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                user.Id = conn.ExecuteScalar<int>(query,
                    new
                    {
                        RoleId = user.RoleId,
                        Login = user.Login,
                        Password = user.Password,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CreatedDate = user.CreatedDate,
                        UpdatedDate = user.UpdatedDate,
                        Ghost = user.Ghost
                    });
            }
        }

        public int Delete(int id)
        {
            int rowsAffected = 0;
            string query = @"DELETE FROM Users
                                WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                rowsAffected = conn.Execute(query, new { Id = id });
            }

            return rowsAffected;
        }

        public User Get(int id)
        {
            User user = null;
            string query = @"SELECT u.Id, u.RoleId, u.Login, u.Password, u.FirstName, 
                                    u.LastName, u.Email, u.CreatedDate, u.UpdatedDate, u.Ghost,
                                    ur.Id ur_id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM Users u
                                JOIN UsersRoles ur ON ur.Id = u.RoleId
                                WHERE u.Id = @id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                user = conn.Query<User, UserRole, User>(
                    sql: query,
                    map: (u, role) => { u.Role = role; return u; },
                    param: new { Id = id },
                    splitOn: "ur_id")
                    .FirstOrDefault();
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users;
            string query = @"SELECT u.Id, u.RoleId, u.Login, u.Password, u.FirstName, 
                                    u.LastName, u.Email, u.CreatedDate, u.UpdatedDate, u.Ghost,
                                    ur.Id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM Users u
                                JOIN UsersRoles ur ON ur.Id = u.RoleId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                users = conn.Query<User, UserRole, User>(query, map: (user, role) => { user.Role = role; return user; });
            }

            return users;
        }

        public int Update(User user)
        {
            int rowsAffected = 0;
            string query = @"UPDATE Users
                                SET RoleId = @RoleId, Login = @Login, Password = @Password, FirstName = @FirstName, 
                                    LastName = @LastName, Email = @Email, 
                                    CreatedDate = @CreatedDate, UpdatedDate = @UpdatedDate, Ghost = @Ghost
                                WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                rowsAffected = conn.Execute(query, new
                {
                    RoleId = user.RoleId,
                    Login = user.Login,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate,
                    Ghost = user.Ghost,
                    Id = user.Id
                });
            }

            return rowsAffected;
        }
    }
}
