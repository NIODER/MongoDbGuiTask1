using Database;
using Database.Entities;
using System;
using System.Collections.Generic;

namespace MongoDbGuiTask1.Model
{
    internal class DatabaseInteractor
    {
		private static string? _connectionString;
		private static DatabaseInteractor? _instance;

		private DatabaseDriver _databaseDriver;
		private readonly string dbName = "shop";

        private DatabaseInteractor(string connectionString)
        {
			_databaseDriver = new DatabaseDriver(connectionString);
        }

		public static DatabaseInteractor Instance()
		{
			return _instance ??= new DatabaseInteractor(ConnectionString);
		}

		public static string ConnectionString
		{
			get 
			{
				return _connectionString ?? throw new NullReferenceException($"{nameof(ConnectionString)} is null or empty"); 
			}
			set 
			{
				_connectionString ??= value;
			}
		}

		public List<string> Collections(string databaseName)
		{
			return _databaseDriver.GetCollectionNames(databaseName);
		}

		public List<string> Databases
		{
			get => _databaseDriver.GetDatabaseNames();
		}

		public IEnumerable<DbEntity> GetCollectionEntities(string collectionName, int page) => collectionName switch
		{
			"items" => _databaseDriver.GetEntitiesPage<Item>(dbName, collectionName, e => true, page),
			"categories" => _databaseDriver.GetEntitiesPage<Category>(dbName, collectionName, e => true, page),
			"employee" => _databaseDriver.GetEntitiesPage<Employee>(dbName, collectionName, e => true, page),
			"order" => _databaseDriver.GetEntitiesPage<Order>(dbName, collectionName, e => true, page),
			_ => throw new InvalidCastException()
		};
	}
}
