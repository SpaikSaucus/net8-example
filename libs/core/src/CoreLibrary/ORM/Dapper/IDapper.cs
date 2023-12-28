namespace CoreLibrary.ORM.Dapper
{
    public interface IDapper
    {
        public IEnumerable<TEntity> GetEntity<TEntity>(string sql);
        public IList<dynamic> QueryMultipleResults(string sql, object param);
        public (IEnumerable<T1>, IEnumerable<T2>) QueryMultipleResults<T1, T2>(string sql, object param);
        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) QueryMultipleResults<T1, T2, T3>(string sql, object param);
    }
}
