using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
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

        public static CountryManager Create(IdentityFactoryOptions<CountryManager> options,
            IOwinContext context)
        {
            var manager =
                new CountryManager(new CountryStore(context.Get<ApplicationIdentityContext>().Countries));
            return manager;
        }
        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _countryStore.GetCountriesAsync();
        }
      
        public void Dispose()
        {
        }
    }
}