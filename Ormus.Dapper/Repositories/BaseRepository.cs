using System;

namespace Ormus.Dapper.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected object ParameterForCmd(object o)
        {
            return o ?? DBNull.Value;
        }
    }
}
