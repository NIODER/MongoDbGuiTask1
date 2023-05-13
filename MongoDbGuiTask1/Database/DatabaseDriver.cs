using Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Database
{
    public class DatabaseDriver
    {
        private static string? _connectionString;
        private readonly MongoClient _client;

        public DatabaseDriver(string connectionString)
        {
            _client = new MongoClient(connectionString);
        }

        public static string ConnectionString
        {
            get
            {
                return _connectionString ?? throw new NullReferenceException();
            }
            set
            {
                _connectionString ??= value;
            }
        }

        public void CreateDatabase(string databaseName, List<string> collections)
        {
            var db = _client.GetDatabase(databaseName);
            Task.WaitAll(collections.Select(col => db.CreateCollectionAsync(col)).ToArray());
        }

        public string? GetDatabaseName(string connectionString)
        {
            return new MongoUrl(connectionString).DatabaseName;
        }

        public List<string> GetDatabaseNames()
        {
            return _client.ListDatabaseNames().ToList();
        }

        public void AddCollection(string databaseName, string collectionName)
        {
            _client.GetDatabase(databaseName)
                .CreateCollection(collectionName);
        }

        public List<string> GetCollectionNames(string databaseName)
        {
            return _client.GetDatabase(databaseName).ListCollectionNames().ToList();
        }

        public long GetCollectionEntitiesCount(string databaseName, string collectionName)
        {
            return _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .CountDocuments(doc => true);
        }

        public List<DbEntity> GetEntitiesPage<DbEntity>(
            string databaseName,
            string collectionName,
            Expression<Func<DbEntity, bool>> filter,
            int pageNumber = 0,
            int pageSize = 10) where DbEntity : Entities.DbEntity => _client
                .GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .Find(filter)
                .Skip(pageNumber * pageSize)
                .Limit(pageSize)
                .ToList();

        public List<DbEntity> GetEntitiesPage<DbEntity>(
            string databaseName,
            string collectionName,
            FilterDefinition<DbEntity> filter,
            int pageNumber = 0,
            int pageSize = 10) where DbEntity : Entities.DbEntity => _client
                .GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .Find(filter)
                .Skip(pageNumber * pageSize)
                .Limit(pageSize)
                .ToList();

        public void InsertOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity entity) where DbEntity : Entities.DbEntity
        {
            _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .InsertOne(entity);
        }

        public UpdateResult? UpdateOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity entity) where DbEntity : Entities.DbEntity
        {
            return _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .UpdateOne(doc => doc.Id == entity.Id, new BsonDocument("$set", entity.ToBsonDocument()));
        }

        public void DeleteOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity dbEntity) where DbEntity : Entities.DbEntity
        {
            _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .FindOneAndDelete(dbEntity.ToBsonDocument());
        }
    }
}
