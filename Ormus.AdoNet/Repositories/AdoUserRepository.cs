using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Ormus.AdoNet.Repositories
{
    public class AdoUserRepository : BaseRepository, IUserRepository
    {
        public AdoUserRepository(string connectionString) : base(connectionString)
        {}

        public void Add(User user)
        {
            string query = @"INSERT INTO Users (RoleId, Login, Password, FirstName, 
                                    LastName, Email, CreatedDate, UpdatedDate, Ghost) VALUES
                        (@RoleId, @Login, @Password, @FirstName, 
                        @LastName, @Email, @CreatedDate, @UpdatedDate, @Ghost);

                        SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@FirstName", ParameterForCmd(user.FirstName));
                    cmd.Parameters.AddWithValue("@LastName", ParameterForCmd(user.LastName));
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                    cmd.Parameters.AddWithValue("@UpdatedDate", ParameterForCmd(user.UpdatedDate));
                    cmd.Parameters.AddWithValue("@Ghost", user.Ghost);

                    cmd.Connection.Open();

                    object o = cmd.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                    {
                        user.Id = Convert.ToInt32(o);
                    }
                }
            }
        }

        public int Delete(int id)
        {
            int rowsAffected = 0;
            string query = @"DELETE FROM Users
                                WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.Connection.Open();

                    object o = cmd.ExecuteNonQuery();
                    if (o != null && o != DBNull.Value)
                    {
                        rowsAffected = Convert.ToInt32(o);
                    }
                }
            }

            return rowsAffected;
        }

        public User Get(int id)
        {
            User user = null;
            string query = @"SELECT u.Id, u.RoleId, u.Login, u.Password, u.FirstName, 
                                    u.LastName, u.Email, u.CreatedDate, u.UpdatedDate, u.Ghost,
                                    ur.Id ur_id, ur.Code ur_code, ur.Description ur_description, ur.CreatedDate ur_createdDate, ur.Ghost ur_ghost
                            FROM Users u
                                JOIN UsersRoles ur ON ur.Id = u.RoleId
                                WHERE u.Id = @id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        user = ExtractUserFromDataReader(dr);
                    }
                }
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();
            string query = @"SELECT u.Id, u.RoleId, u.Login, u.Password, u.FirstName, 
                                    u.LastName, u.Email, u.CreatedDate, u.UpdatedDate, u.Ghost,
                                    ur.Id ur_id, ur.Code ur_code, ur.Description ur_description, ur.CreatedDate ur_createdDate, ur.Ghost ur_ghost
                            FROM Users u
                                JOIN UsersRoles ur ON ur.Id = u.RoleId;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        User u = ExtractUserFromDataReader(dr);
                        users.Add(u);
                    }
                }
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
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoleId", user.RoleId);
                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@FirstName", ParameterForCmd(user.FirstName));
                    cmd.Parameters.AddWithValue("@LastName", ParameterForCmd(user.LastName));
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                    cmd.Parameters.AddWithValue("@UpdatedDate", user.UpdatedDate);
                    cmd.Parameters.AddWithValue("@Ghost", user.Ghost);

                    cmd.Parameters.AddWithValue("@Id", user.Id);

                    cmd.Connection.Open();

                    object o = cmd.ExecuteNonQuery();
                    if (o != null && o != DBNull.Value)
                    {
                        rowsAffected = Convert.ToInt32(o);
                    }
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Extracts User from DataReader
        /// </summary>
        private User ExtractUserFromDataReader(SqlDataReader dr)
        {
            User user = new User();
            user.Id = Convert.ToInt32(dr["Id"]);
            user.Login = dr["Login"].ToString();
            user.Password = dr["Password"].ToString();
            user.FirstName = dr["FirstName"].ToString();
            user.LastName = dr["LastName"].ToString();
            user.Email = dr["Email"].ToString();
            user.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            user.UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : (DateTime?)null;
            user.Ghost = Convert.ToBoolean(dr["Ghost"]);
            user.Role = new UserRole()
            {
                Id = Convert.ToInt32(dr["ur_id"]),
                Code = dr["ur_code"].ToString(),
                Description = dr["ur_description"].ToString(),
                CreatedDate = Convert.ToDateTime(dr["ur_createdDate"]),
                Ghost = Convert.ToBoolean(dr["ur_ghost"])
            };

            return user;
        }
    }
}
