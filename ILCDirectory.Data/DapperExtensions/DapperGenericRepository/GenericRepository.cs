using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncDapperExtensions;
using DapperGenericRepository.Helpers;
using System.Data.SqlClient;

namespace DapperGenericRepository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private static readonly IEnumerable<string> _listOfProperties;
        private static string _selectFields = string.Empty;
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly string _idName;

        static GenericRepository()
        {
            _listOfProperties = GenerateListOfProperties(typeof(T).GetProperties());
        }

        protected GenericRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _idName = $"{tableName}Id";
        }

        protected GenericRepository(string connectionString, string tableName, string idName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _idName = idName;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await GenericRepository<T>.LoadFields(async () =>
            {
                using var connection = CreateConnection();
                return await connection.QueryAsyncWithToken<T>($"SELECT {_selectFields} FROM {_tableName}",
                    cancellationToken: cancellationToken);
            }, _idName, true);
        }

        public async Task<T> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            return await GenericRepository<T>.LoadFields(async () =>
            {
                using var connection = CreateConnection();
                var result =
                    await connection.QuerySingleOrDefaultWithToken<T>($"SELECT {_selectFields} FROM {_tableName} WHERE {_idName}=@Id",
                        new { Id = id }, cancellationToken: cancellationToken);
                if (result == null) throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

                return result;
            }, _idName, true);
        }

        public async Task InsertAsync(T t, CancellationToken cancellationToken = default)
        {
            var insertQuery = GenerateInsertQuery(_idName);

            using var connection = CreateConnection();
            var result = await connection.ExecuteAsyncWithToken(insertQuery, t, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            Console.Write(result);
        }

        //The bulk copy operation occurs in a non-transacted way, with no opportunity for rolling it back.
        public void InsertBulk(IEnumerable<T> items)
        {
            var dataTable = BulkInsertHelpers.CreateDataTable<T>(items);
            using var bulkInsert = new SqlBulkCopy(_connectionString);
            bulkInsert.DestinationTableName = _tableName;
            bulkInsert.WriteToServer(dataTable);
        }

        public async Task UpdateAsync(T t, CancellationToken cancellationToken = default)
        {
            var updateQuery = GenerateUpdateQuery(_idName);

            using var connection = CreateConnection();
            await connection.ExecuteAsyncWithToken(updateQuery, t, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsyncWithToken($"DELETE FROM {_tableName} WHERE {_idName}=@Id", new { Id = id },
                cancellationToken: cancellationToken);
        }

        //uses transaction
        public async Task InsertRangeAsync(IEnumerable<T> t, CancellationToken cancellationToken = default)
        {
            var insertQuery = GenerateInsertQuery(_idName);

            using var dbConnection = CreateConnection();
            using var tran = dbConnection.BeginTransaction();

            try
            {
                await dbConnection.ExecuteAsyncWithToken(insertQuery, t, tran, cancellationToken: cancellationToken);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        #region PrivateHelperMethods

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Open new connection and return it for use
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }

        private static TU LoadFields<TU>(Func<TU> method, string idName, bool includeId = false)
        {
            if (string.IsNullOrEmpty(_selectFields)) _selectFields = GenericRepository<T>.GenerateSelectFields(_listOfProperties, includeId, idName);
            return method.Invoke();
        }

        private static IEnumerable<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                select prop.Name).ToList();
        }

        private static void IgnoreId(string property, Action action, string _idName)
        {
            if (!property.Equals(_idName)) action.Invoke();
        }

        private static string GenerateSelectFields(IEnumerable<string> properties, bool includeId, string idName)
        {
            var fields = new StringBuilder();
            foreach (var property in properties)
            {
                if (includeId == false)
                {
                    GenericRepository<T>.IgnoreId(property, () => { fields.Append($"{property},"); }, idName);
                }
                else
                {
                    fields.Append($"{property},");
                }
            }

            fields.Remove(fields.Length - 1, 1);
            return fields.ToString();
        }

        private string GenerateInsertQuery(string idName)
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append('(');
            foreach (var listOfProperty in _listOfProperties)
            {
                //TODO: extract this check
                GenericRepository<T>.IgnoreId(listOfProperty, () => { insertQuery.Append($"[{listOfProperty}],"); }, _idName);
            }

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");

            foreach (var listOfProperty in _listOfProperties)
            {
                GenericRepository<T>.IgnoreId(listOfProperty, () => { insertQuery.Append($"@{listOfProperty},"); }, _idName);
            }

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(')');

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery(string idName)
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");

            foreach (var listOfProperty in _listOfProperties)
            {
                GenericRepository<T>.IgnoreId(listOfProperty, () => { updateQuery.Append($"{listOfProperty}=@{listOfProperty},"); }, _idName);
            }

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append($" WHERE {idName}=@Id");

            return updateQuery.ToString();
        }

        #endregion
    }
}