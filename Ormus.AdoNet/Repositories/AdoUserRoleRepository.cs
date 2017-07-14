using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Ormus.AdoNet.Repositories
{
    public class AdoUserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public AdoUserRoleRepository(string connectionString) : base(connectionString)
        {}

        public void Add(UserRole userRole)
        {
            string query = @"INSERT INTO UsersRoles (Code, Description, CreatedDate, Ghost) VALUES
                        (@Code, @Description, @CreatedDate, @Ghost);

                        SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Code", userRole.Code);
                    cmd.Parameters.AddWithValue("@Description", userRole.Description);
                    cmd.Parameters.AddWithValue("@CreatedDate", userRole.CreatedDate);
                    cmd.Parameters.AddWithValue("@Ghost", userRole.Ghost);

                    cmd.Connection.Open();

                    object o = cmd.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                    {
                        userRole.Id = Convert.ToInt32(o);
                    }
                }
            }
        }

        public int Delete(int id)
        {
            int rowsAffected = 0;
            string query = @"DELETE FROM UsersRoles
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

        public UserRole Get(int id)
        {
            UserRole userRole = null;

            string query = @"SELECT ur.Id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM UsersRoles ur 
                            WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        userRole = ExtractUserRoleFromDataReader(dr);
                    }
                }
            }

            return userRole;
        }

        public IEnumerable<UserRole> GetAll()
        {
            List<UserRole> userRoles = new List<UserRole>();
            string query = @"SELECT ur.Id, ur.Code, ur.Description, ur.CreatedDate, ur.Ghost
                            FROM UsersRoles ur;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        UserRole u = ExtractUserRoleFromDataReader(dr);
                        userRoles.Add(u);
                    }
                }
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
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Code", userRole.Code);
                    cmd.Parameters.AddWithValue("@Description", userRole.Description);
                    cmd.Parameters.AddWithValue("@Ghost", userRole.Ghost);
                    cmd.Parameters.AddWithValue("@Id", userRole.Id);
                    cmd.Parameters.AddWithValue("@CreatedDate", userRole.CreatedDate);

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
        /// Extracts UserRole from DataReader
        /// </summary>
        private UserRole ExtractUserRoleFromDataReader(SqlDataReader dr)
        {
            UserRole userRole = new UserRole();
            userRole.Id = Convert.ToInt32(dr["Id"]);
            userRole.Code = dr["Code"].ToString();
            userRole.Description = dr["Description"].ToString();
            userRole.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            userRole.Ghost = Convert.ToBoolean(dr["Ghost"]);

            return userRole;
        }
    }
}
