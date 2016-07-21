using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MongoDB.Driver;

namespace OneVietnam.DAL
{
    public abstract class AbstractManager<T> : IDisposable
    {
        protected internal AbstractStore<T> Store { get; set; }

        protected AbstractManager(AbstractStore<T> store)
        {
            Store = store;
        }

        public virtual async Task CreatAsync(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            await Store.CreateAsync(instance).ConfigureAwait(false);
        }

        public virtual async Task<T> FindByIdAsync(string id)
        {
           return await Store.FindByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(T instance)
        {
            if (instance==null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            await Store.UpdateAsync(instance).ConfigureAwait(false);
        }
        public virtual async Task UpdateAsync(T instance,bool upsert)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            await Store.UpdateAsync(instance,upsert).ConfigureAwait(false);
        }

        public virtual async Task DeleteByIdAsync(string id)
        {
            await Store.DeleteByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> FindAllAsync(BaseFilter baseFilter, FilterDefinition<T> filter)
        {
            return await Store.FindAllAsync(baseFilter, filter).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> FindAllAsync(BaseFilter baseFilter, bool deletedFlag)
        {
            return await Store.FindAllAsync(baseFilter, deletedFlag).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> FindAllAsync(FilterDefinition<T> filter)
        {
            return await Store.FindAllAsync(filter);
        }

        public virtual async Task<List<T>> FindAllAsync(bool deletedFlag)
        {
            return await Store.FindAllAsync(deletedFlag).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> FindAllAsync()
        {
            return await Store.FindAllAsync().ConfigureAwait(false);
        }

        public virtual async Task<List<T>> FindAllAsync(BaseFilter baseFilter)
        {
            return await Store.FindAllAsync(baseFilter).ConfigureAwait(false);
        }
        public void Dispose()
        {            
        }
    }
}