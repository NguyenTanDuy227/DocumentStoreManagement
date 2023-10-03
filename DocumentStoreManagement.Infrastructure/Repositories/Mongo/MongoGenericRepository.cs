﻿using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DocumentStoreManagement.Infrastructure.Repositories.Mongo
{
    public class MongoGenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoDatabase database;
        protected readonly IMongoCollection<T> dbSet;

        public MongoGenericRepository(IMongoApplicationContext context)
        {
            database = context.Database;
            dbSet = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.InsertOneAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.InsertManyAsync(entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await dbSet.Find(x => x.Id == (string)id).FirstOrDefaultAsync();
        }

        public IEnumerable<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(T entity)
        {
            await dbSet.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(T entity)
        {
            await dbSet.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        public async Task<bool> CheckExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await dbSet.Find(expression).AnyAsync();
        }
    }
}
