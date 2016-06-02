using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class CountryStore
    {
        private readonly IMongoCollection<Country> _countries;

        public CountryStore(IMongoCollection<Country> countries)
        {
            _countries = countries;
        }

        public Task CreatAsync(Country country)
        {
            return _countries.InsertOneAsync(country);
        }
        public virtual Task UpdateAsync(Country country)
        {
            return (Task)_countries.ReplaceOneAsync<Country>((Expression<Func<Country, bool>>)(c => c.Id == country.Id), country, (UpdateOptions)null);
        }
    }
}