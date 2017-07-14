using System;

namespace Ormus.AdoNet.Repositories
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
