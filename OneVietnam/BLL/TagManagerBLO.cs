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
    public class TagManager: IDisposable
    {
        private readonly TagStore _tagStore;
        public TagManager(TagStore tagStore)
        {
            _tagStore = tagStore;
        }
        public  Task CreateAsync(Tag pTag)
        {            
           return  _tagStore.CreatAsync(pTag);
        }

        public static TagManager Create(IdentityFactoryOptions<TagManager> options,
            IOwinContext context)
        {
            var manager =
                new TagManager(new TagStore(context.Get<ApplicationIdentityContext>().Tags));
            return manager;
        }
        public  Task<List<Tag>> GetTagsAsync()
        {
            return  _tagStore.GetTagsAsync();
        }

        public List<string> GetTagsValueAsync()
        {
            try
            {               
                return  _tagStore.GetTagsValueAsync();
            }catch(Exception)
            {
                return null;
            }
        }

        public  Task<List<Tag>> FindTagByValueAsync(string pTagValue)
        {
            return  _tagStore.FindTagByValueAsync(pTagValue);
        }

        public void Dispose()
        {
        }
    }
}