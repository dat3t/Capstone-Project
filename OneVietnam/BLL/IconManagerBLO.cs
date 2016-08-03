using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class IconManager : AbstractManager<Icon>
    {        
        public static IconManager Create(IdentityFactoryOptions<IconManager> options,
            IOwinContext context)
        {
            var manager =
                new IconManager(new IconStore(context.Get<ApplicationIdentityContext>().Icons));
            return manager;
        }                

        public async Task<List<Icon>> GetIconGender()
        {
            var list = await Store.FindAllAsync();
            return list?.Where(icon => string.Equals(icon.IconType, Constants.IconTypeGender)).ToList();
        }

        public async Task<List<Icon>> GetIconPostAsync()
        {
            var list = await Store.FindAllAsync();
            return list?.Where(icon => string.Equals(icon.IconType, Constants.IconTypePost)).ToList();
        }
        public IconManager(AbstractStore<Icon> store) : base(store)
        {
        }
    }
}