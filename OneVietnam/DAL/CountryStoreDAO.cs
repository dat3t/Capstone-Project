using System.Collections.Generic;
using System.Threading.Tasks;
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
            return _countries.ReplaceOneAsync(c => c.Id == country.Id, country, null);
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _countries.Find(u => true).ToListAsync();
        }
    }
}