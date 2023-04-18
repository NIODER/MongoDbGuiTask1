using MongoDB.Driver;
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

        public string ConnectionString
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

        public List<string> GetDatabaseNames()
        {
            return _client.ListDatabaseNames().ToList();
        }

        public List<string> GetCollectionNames(string databaseName)
        {
            return _client.GetDatabase(databaseName).ListCollectionNames().ToList();
        }

        public List<DbEntity> GetEntitiesPage<DbEntity>(
            string databaseName,
            string collectionName,
            Expression<Func<DbEntity, bool>> filter,
            int pageNumber = 0) where DbEntity : Entities.DbEntity => _client
                .GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .Find(filter)
                .Skip(pageNumber * 10)
                .Limit(10)
                .ToList();

        public void InsertOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity entity) where DbEntity : Entities.DbEntity
        {
            _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .InsertOne(entity);
        }

        public void UpdateOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity oldEntity, DbEntity newEntity) where DbEntity : Entities.DbEntity
        {
            throw new NotImplementedException();
        }

        public void DeleteOneEntity<DbEntity>(string databaseName, string collectionName, DbEntity dbEntity) where DbEntity : Entities.DbEntity
        {
            _client.GetDatabase(databaseName)
                .GetCollection<DbEntity>(collectionName)
                .DeleteOne(entity => entity.Equals(dbEntity));
        }
    }
}
