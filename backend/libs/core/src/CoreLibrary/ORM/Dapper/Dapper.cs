using Dapper;
using System.Data;

namespace CoreLibrary.ORM.Dapper
{
    public class Dapper(IDbConnection connection) : IDapper
    {
        private readonly IDbConnection connection = connection;

        public IEnumerable<TEntity> GetEntity<TEntity>(string sql)
        {
            return this.connection.Query<TEntity>(sql);
        }

        public IList<dynamic> QueryMultipleResults(string sql, object param)
        {
            var objects = new List<dynamic>();

            var grid = this.connection.QueryMultiple(sql, param, commandType: CommandType.StoredProcedure);

            while (!grid.IsConsumed)
            {
                objects.Add(grid.Read<dynamic>());
            }

            return objects;
        }

        public (IEnumerable<T1>, IEnumerable<T2>) QueryMultipleResults<T1, T2>(string sql, object param)
        {
            var grid = this.connection.QueryMultiple(sql, param, commandType: CommandType.StoredProcedure);

            var t1 = grid.Read<T1>();
            var t2 = grid.Read<T2>();

            return (t1, t2);
        }

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) QueryMultipleResults<T1, T2, T3>(string sql, object param)
        {
            var grid = this.connection.QueryMultiple(sql, param, commandType: CommandType.StoredProcedure);

            var t1 = grid.Read<T1>();
            var t2 = grid.Read<T2>();
            var t3 = grid.Read<T3>();

            return (t1, t2, t3);
        }
    }
}
