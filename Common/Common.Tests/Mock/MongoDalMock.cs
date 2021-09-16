using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.DbModels;
using Common.Utils;

namespace Common.Tests.Mock
{
     public class MongoDalMock: MongoDal
    {
        private static Dictionary<Type, object> _globalDbs = new Dictionary<Type, object>();
        private Dictionary<Type, object> _collections = new Dictionary<Type, object>();

        public MongoDatabaseBase Db { get; set; }

        protected MongoDalMock(MongoDatabaseBase db) : 
            base(db)
        {
            Db = db;
        }

        public static async Task<MongoDalMock> GetMockEntitiesDalGlobalAsync<TDatabase>()
            where TDatabase : MongoDatabaseBase, new()
        {
            if (_globalDbs.TryGetValue(typeof(TDatabase), out var db))
                return (MongoDalMock)db;
            var dal = _globalDbs[typeof(TDatabase)] =
                await GetMockMongoDalAsync<TDatabase>();
            return (MongoDalMock)dal;
        }

        public static async Task<MongoDalMock> GetMockMongoDalAsync<TDatabase>(
            string dbName = "test",
            string connectionString = "mongodb://localhost:27017")
            where TDatabase: MongoDatabaseBase, new()
        {
            var db = new TestDb<TDatabase>();
            await db.ConnectAsync(connectionString, dbName);
            var dal = new MongoDalMock(db.Database);
            return dal;
        }
    }
}