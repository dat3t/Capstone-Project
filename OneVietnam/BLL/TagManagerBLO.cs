using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
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

        public static TagManager Create(IdentityFactoryOptions<TagManager> options,
            IOwinContext context)
        {
            var manager =
                new TagManager(new TagStore(context.Get<ApplicationIdentityContext>().Tags));
            return manager;
        }

        public  Task CreateAsync(Tag pTag)
        {            
           return  _tagStore.CreatAsync(pTag);
        }
        
        public Task<List<Tag>> GetTagsAsync()
        {
            return  _tagStore.GetTagsAsync();
        }
        public List<string> GetTagsValueAsync()
        {
            var tagList = GetTagsAsync();
            if (tagList!= null && tagList.Result.Count > 0)
            {
                return tagList.Result.Select(tagItem => tagItem.TagValue).ToList();
            }
            else
            {
                return null;
            }

        }

        public List<Tag> FindTagByValueAsync(string pTagValue)
        {
            var listTags =  _tagStore.FindTagByValueAsync(pTagValue);
            return listTags.Result;
        }

        public void Dispose()
        {
        }
    }
}