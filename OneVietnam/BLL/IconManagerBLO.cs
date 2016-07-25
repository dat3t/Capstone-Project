using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class IconManager : IDisposable
    {
        private readonly IconStore _iconStore;
        public IconManager(IconStore pIconStore)
        {
            _iconStore = pIconStore;
        }

        public static IconManager Create(IdentityFactoryOptions<IconManager> options,
            IOwinContext context)
        {
            var manager =
                new IconManager(new IconStore(context.Get<ApplicationIdentityContext>().Icons));
            return manager;
        }
        public Task CreateAsync(Icon pIcon)
        {
            return _iconStore.CreatAsync(pIcon);
        }

        public Task<List<Icon>> GetIconsAsync()
        {
            return _iconStore.GetIconsAsync();
        }


        public List<Icon> GetIconGenderAndSosAsync()
        {
            var list = GetIconsAsync();
            if (list != null)
            {
                List<Icon> genderAndSos = new List<Icon>();

                foreach (var icon in list.Result)
                {
                    if (string.Equals(icon.IconType, Constants.IconTypeGender) | string.Equals(icon.IconType, Constants.IconTypeSos))
                    {
                        genderAndSos.Add(icon);
                    }
                }
                return genderAndSos;
            }
            return null;
        }

        public List<Icon> GetIconPostAsync()
        {
            var list = GetIconsAsync();
            if (list != null)
            {
                List<Icon> postTypes = new List<Icon>();

                foreach (var icon in list.Result)
                {
                    if (string.Equals(icon.IconType, Constants.IconTypePost))
                    {
                        postTypes.Add(icon);
                    }
                }
                return postTypes;
            }
            return null;
        }

        public void Dispose()
        {
        }
    }
}