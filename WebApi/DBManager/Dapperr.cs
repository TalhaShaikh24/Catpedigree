

using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace WebApi.DBManager
{
    public class Dapperr : IDapper
    {

        private string Connectionstring;
      
        public Dapperr(IConfiguration configuration)
        {

             Connectionstring = configuration.GetConnectionString("dbconnection");
            //Connectionstring = "Data Source=DESKTOP-554BILU;Initial Catalog=CatPedigree_db;Integrated Security=True;";



        }
        public void Dispose()
        {

        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            int result;
            IDbConnection db = new SqlConnection(Connectionstring);
            try
            {
                

                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = db.Execute(sp, parms, commandType: commandType, transaction: tran,commandTimeout: 90);
                    tran.Commit();
                }
                catch (DbException ex)
                {
                    tran.Rollback();
                      throw ex;
                }
            }
            catch (Exception ex)
            {
                  throw new Exception(ex.Message);
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(Connectionstring);
            var res = db.Query<T>(sp, parms, commandType: commandType, commandTimeout: 90).FirstOrDefault();
            db.Close();
            return res;
        }
        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            // Use SqlConnection directly since it supports OpenAsync
            using (var db = new SqlConnection(Connectionstring))
            {
                await db.OpenAsync(); // Open the connection asynchronously

                // Execute the query asynchronously
                var result = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, commandTimeout: 90);

                return result;
            }
        }


        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(Connectionstring);
            var res = db.Query<T>(sp, parms, commandType: commandType, commandTimeout: 90).ToList();
            db.Close();
            return res;
        }

        public GridReader GetMultiple(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(Connectionstring);
            var res = db.QueryMultiple(sp, parms, commandType: commandType, commandTimeout: 90);


            db.Close();
            return res;
        }


        public DbConnection GetDbconnection()
        {
            return new SqlConnection(Connectionstring);
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(Connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran, commandTimeout: 90).FirstOrDefault();
                    tran.Commit();
                }
                catch (DbException ex)
                {
                    tran.Rollback();

                      throw ex;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        //public T InsertWithTableValued<T>(string sp, object, CommandType commandType = CommandType.StoredProcedure)
        //{
        //    T result;
        //    IDbConnection db = new SqlConnection(Connectionstring);
        //    try
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        var tran = db.BeginTransaction();
        //        try
        //        {
        //            result = db.Execute( (sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
        //            tran.Commit();
        //        }
        //        catch (DbException ex)
        //        {
        //            tran.Rollback();
        //              throw new Exception(ex.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //          throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (db.State == ConnectionState.Open)
        //            db.Close();
        //    }

        //    return result;
        //}

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(Connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran, commandTimeout: 90).FirstOrDefault();
                    tran.Commit();
                }
                catch (DbException ex)
                {
                    tran.Rollback();

                    throw ex;
                }
            }
            catch (Exception ex)
            {
                  throw new Exception(ex.Message);
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }


        //public Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultiple<T1, T2>(string sql, object parameters,
        //                                Func<Dapper.SqlMapper.GridReader, IEnumerable<T1>> func1,
        //                                Func<Dapper.SqlMapper.GridReader, IEnumerable<T2>> func2)
        //{
        //    var objs = getMultiple(sql, parameters, func1, func2);
        //    return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        //}

        //public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultiple<T1, T2, T3>(string sql, object parameters,
        //                                Func<GridReader, IEnumerable<T1>> func1,
        //                                Func<GridReader, IEnumerable<T2>> func2,
        //                                Func<GridReader, IEnumerable<T3>> func3)
        //{
        //    var objs = getMultiple(sql, parameters, func1, func2, func3);
        //    return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        //}

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>> GetMultipleObjects<T1, T2, T3, T4, T5, T6>(string sql, object parameters,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T2>> func2,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T3>> func3,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T4>> func4,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T5>> func5,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T6>> func6)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3, func4, func5, func6);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>, objs[5] as IEnumerable<T6>);
        }




        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> GetMultipleObjects<T1, T2, T3, T4, T5>(string sql, object parameters,
                                         Func<Dapper.SqlMapper.GridReader, IEnumerable<T1>> func1,
                                         Func<Dapper.SqlMapper.GridReader, IEnumerable<T2>> func2,
                                         Func<Dapper.SqlMapper.GridReader, IEnumerable<T3>> func3,
                                         Func<Dapper.SqlMapper.GridReader, IEnumerable<T4>> func4,
                                         Func<Dapper.SqlMapper.GridReader, IEnumerable<T5>> func5)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3, func4, func5);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>);
        }
        private List<object> GetMultiple(string sql, object parameters, params Func<Dapper.SqlMapper.GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();
            using (IDbConnection db = new SqlConnection(Connectionstring))
            {
                var gridReader = db.QueryMultiple(sql, parameters, null, null, CommandType.StoredProcedure);

                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }

            return returnResults;
        }

        //
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> GetMultipleObjects<T1, T2>(string sql, object parameters,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<Dapper.SqlMapper.GridReader, IEnumerable<T2>> func2
                                       )
        {
            var objs = GetMultiple(sql, parameters, func1, func2);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>);
        }




        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> GetMultipleObjects<T1, T2, T3>(string sql, object parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1, Func<SqlMapper.GridReader, IEnumerable<T2>> func2, Func<SqlMapper.GridReader, IEnumerable<T3>> func3)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> GetMultipleObjects<T1, T2, T3, T4>(string sql, object parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1, Func<SqlMapper.GridReader, IEnumerable<T2>> func2, Func<SqlMapper.GridReader, IEnumerable<T3>> func3, Func<SqlMapper.GridReader, IEnumerable<T4>> func4)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3, func4);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>);

        }


        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>> GetMultipleObjects<T1, T2, T3, T4, T5, T6, T7>(string sql, object parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1, Func<SqlMapper.GridReader, IEnumerable<T2>> func2, Func<SqlMapper.GridReader, IEnumerable<T3>> func3, Func<SqlMapper.GridReader, IEnumerable<T4>> func4, Func<SqlMapper.GridReader, IEnumerable<T5>> func5, Func<SqlMapper.GridReader, IEnumerable<T6>> func6, Func<SqlMapper.GridReader, IEnumerable<T7>> func7)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3, func4, func5, func6, func7);
            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>, objs[5] as IEnumerable<T6>, objs[6] as IEnumerable<T7>);
        }

        public void Insert(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //var result;
            IDbConnection db = new SqlConnection(Connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    db.Query(sp, parms, commandType: commandType, transaction: tran, commandTimeout: 90).FirstOrDefault();
                    tran.Commit();
                }
                catch (DbException ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                  throw new Exception(ex.Message);
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

        }


        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, Tuple<IEnumerable<T7>, IEnumerable<T8>, IEnumerable<T9>>> GetMultipleObjects<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string sql, object parameters, Func<GridReader, IEnumerable<T1>> func1, Func<GridReader, IEnumerable<T2>> func2, Func<GridReader, IEnumerable<T3>> func3, Func<GridReader, IEnumerable<T4>> func4, Func<GridReader, IEnumerable<T5>> func5, Func<GridReader, IEnumerable<T6>> func6, Func<GridReader, IEnumerable<T7>> func7, Func<GridReader, IEnumerable<T8>> func8, Func<GridReader, IEnumerable<T9>> func9)
        {
            var objs = GetMultiple(sql, parameters, func1, func2, func3, func4, func5, func6, func7, func8, func9);

            return Tuple.Create(objs[0] as IEnumerable<T1>, objs[1] as IEnumerable<T2>, objs[2] as IEnumerable<T3>, objs[3] as IEnumerable<T4>, objs[4] as IEnumerable<T5>, objs[5] as IEnumerable<T6>, Tuple.Create(objs[6] as IEnumerable<T7>, objs[7] as IEnumerable<T8>, objs[8] as IEnumerable<T9>));


        }
    }
}
