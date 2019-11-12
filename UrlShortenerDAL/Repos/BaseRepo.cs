using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UrlShortenerDAL.EF;

namespace UrlShortenerDAL.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : class, new()
    {
        private readonly DbSet<T> _table;
        private readonly DbContext _db;
        //protected UrlShortenerContext Context => _db;
        //public BaseRepo() : this(new UrlShortenerContext()) { }

        public BaseRepo(DbContext context)
        {
            _db = context;
            _table = _db.Set<T>();
        }

        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }
        public int Add(IList<T> entities)
        {
            _table.AddRange(entities);
            return SaveChanges();
        }

        public int Update(T entity)
        {
            _table.Update(entity);
            return SaveChanges();
        }
        public int Update(IList<T> entities)
        {
            _table.UpdateRange(entities);
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }
        public T GetOne(int? id) => _table.Find(id);
        public T GetOne(Expression<Func<T, bool>> where) => _table.Where(where).First();
        public virtual List<T> GetAll() => _table.ToList();

        public List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending) =>
            (ascending ? _table.OrderBy(orderBy) : _table.OrderByDescending(orderBy)).ToList();
        public List<T> GetSome(Expression<Func<T, bool>> where) =>
            _table.Where(where).ToList();

        internal int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //Thrown when there is a concurrency error
                //for now, just rethrow the exception
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //Thrown when max retries have been hit
                //Examine the inner exception(s) for additional details
                //for now, just rethrow the exception
                throw;
            }
            catch (DbUpdateException ex)
            {
                //Thrown when database update fails
                //Examine the inner exception(s) for additional
                //details and affected objects
                //for now, just rethrow the exception
                throw;
            }
            catch (Exception ex)
            {
                //some other exception happened and should be handled
                throw;
            }
        }

        public List<T> ExecuteQuery(string sql) => _table.FromSqlRaw(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
            => _table.FromSqlRaw(sql, sqlParametersObjects).ToList();

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
