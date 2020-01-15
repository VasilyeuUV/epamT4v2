using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public abstract class RepositoryBase<T> : IDisposable, IRepository<T> where T : class
    {
        private SalesContext _context = null;
        protected SalesContext Context { get => _context; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="context"></param>
        internal RepositoryBase(SalesContext context)
        {
            this._context = context;
        }


        public virtual T Get(int id)
        {
            var dbSet = this._context.Set<T>();
            return dbSet.FindAsync(id).Result;
        }

        public virtual TEntity Get<TEntity>(string name) where TEntity : EntityBase
        {
            var dbSet = this._context.Set<TEntity>();
            return dbSet.FirstOrDefaultAsync(x => x.Name.Equals(name)).Result;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this._context.Set<T>();
        }


        /// <summary>
        /// Insert Entity to DB
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual void Insert(T entity)
        {
            this._context.Entry(entity).State = EntityState.Added;
            //var dbSet = this._context.Set<T>();
            //dbSet.Add(item);
        }

        /// <summary>
        /// Delete Entity from DB by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void Delete(int id)
        {
            var dbSet = this._context.Set<T>();
            T entity = dbSet.Find(id);
            if (entity != null)
            {
                this._context.Entry(entity).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Delete Entity from DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Delete(T entity)
        {
            this._context.Entry(entity).State = EntityState.Deleted;
            //var dbSet = this._context.Set<T>();
            //dbSet.Remove(entity);
        }


        public virtual void Update(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
        }


        #region IDISPOSABLE
        //##############################################################################################################

        private bool _disposed = false;
       

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


    }
}
