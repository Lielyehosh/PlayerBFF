using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Utils.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace Common.Utils
{
    public class MongoDal : IMongoDal
    {
        private readonly MongoDatabaseBase _db;
        

        public MongoDal(MongoDatabaseBase db)
        {
            _db = db;
        }

        public IMongoCollection<TDbModel> GetCollection<TDbModel>() where TDbModel : IDbModel
        {
            return _db.GetCollection<TDbModel>();
        }

        public async Task<TDbModel> InsertOneAsync<TDbModel>(TDbModel document, CancellationToken ct) where TDbModel : IDbModel
        {
            document.CreateAt = DateTime.UtcNow;
            document.ModifyAt = DateTime.UtcNow;
            try
            {
                var coll = _db.GetCollection<TDbModel>();
                await coll.InsertOneAsync(document, cancellationToken: ct);
                return document;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to insert document to DB. error-{ErrorMsg}", ex.Message);
                throw;
            }
        }
    }
}