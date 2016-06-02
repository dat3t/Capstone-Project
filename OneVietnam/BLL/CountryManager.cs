using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class CountryManager : IDisposable
    {
        private readonly CountryStore _countryStore;
        public CountryManager(CountryStore countryStore)
        {
            _countryStore = countryStore;
        }
        public async Task CreateAsync(Country country)
        {
            await _countryStore.CreatAsync(country).ConfigureAwait(false);
        }

        public void Dispose()
        {            
        }
    }
}