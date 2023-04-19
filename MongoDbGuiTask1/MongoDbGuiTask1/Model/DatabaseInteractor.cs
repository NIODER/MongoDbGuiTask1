using Amazon.Runtime.Internal.Util;
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

		private readonly DatabaseDriver _databaseDriver;
		private readonly string dbName = "shop";
		public const string ITEMS_COLLECTION_NAME = "items";
		public const string CATEGORIES_COLLECTION_NAME = "categories";
		public const string EMPLOYEE_COLLECTION_NAME = "employee";
		public const string ORDER_COLLECTION_NAME = "order";

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

		public long GetCollectionLength(string collectionName)
		{
			return _databaseDriver.GetCollectionEntitiesCount(dbName, collectionName);
		}

		public IEnumerable<DbEntity> GetCollectionEntities(string collectionName, int page) => collectionName switch
		{
			ITEMS_COLLECTION_NAME => _databaseDriver.GetEntitiesPage<Item>(dbName, collectionName, e => true, page),
			CATEGORIES_COLLECTION_NAME => _databaseDriver.GetEntitiesPage<Category>(dbName, collectionName, e => true, page),
			EMPLOYEE_COLLECTION_NAME => _databaseDriver.GetEntitiesPage<Employee>(dbName, collectionName, e => true, page),
			ORDER_COLLECTION_NAME => _databaseDriver.GetEntitiesPage<Order>(dbName, collectionName, e => true, page),
			_ => throw new InvalidCastException()
		};

		public static string GetCollectionName(DbEntity dbEntity) => dbEntity switch
		{
			Item => ITEMS_COLLECTION_NAME,
			Category => CATEGORIES_COLLECTION_NAME,
			Employee => EMPLOYEE_COLLECTION_NAME,
			Order => ORDER_COLLECTION_NAME,
			_ => throw new InvalidCastException($"dbEntity has no collection setted")
		};

		public void DeleteEntity(DbEntity entity)
		{
			_databaseDriver.DeleteOneEntity(dbName, GetCollectionName(entity), entity);
		}

		public long UpdateEntity(DbEntity entity)
		{
			return _databaseDriver.UpdateOneEntity(dbName, GetCollectionName(entity), entity)?.ModifiedCount ?? 0;
		}

		public void InsertEntity(DbEntity entity)
		{
			_databaseDriver.InsertOneEntity(dbName, GetCollectionName(entity), entity);
		}
	}
}
