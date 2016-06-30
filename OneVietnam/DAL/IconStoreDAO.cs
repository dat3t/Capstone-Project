using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using OneVietnam.BLL;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class IconStore
    {
        private readonly IMongoCollection<Icon> _icons;

        public IconStore(IMongoCollection<Icon> pIcons)
        {
            _icons = pIcons;
        }

        public Task CreatAsync(Icon pIcons)
        {
            return _icons.InsertOneAsync(pIcons);
        }

        public virtual Task UpdateAsync(Icon pIcons)
        {
            return _icons.ReplaceOneAsync(c => c.IconId == pIcons.IconId, null);
        }

        public Task<List<Icon>> GetIconsAsync()
        {
            return _icons.Find(u => true).ToListAsync();
        }
        
    }

}