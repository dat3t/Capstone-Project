using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.DAL;
using Tag = OneVietnam.DTL.Tag;

namespace OneVietnam.BLL
{
    public class TagManager: AbstractManager<Tag>
    {        
        public static TagManager Create(IdentityFactoryOptions<TagManager> options,
            IOwinContext context)
        {
            var manager =
                new TagManager(new TagStore(context.Get<ApplicationIdentityContext>().Tags));
            return manager;
        }
                
        public async Task<List<string>>  GetTagsValueAsync()
        {
            var tagList = await FindAllAsync();
            if (tagList!= null && tagList.Count > 0)
            {
                return tagList.Select(tagItem => tagItem.TagValue).ToList();
            }
            else
            {
                return null;
            }

        }
        public async Task<List<Tag>> FindTagByValueAsync(string pTagValue)
        {
            //var listTags =  _tagStore.FindTagByValueAsync(pTagValue);
            var filter = Builders<Tag>.Filter.Regex("TagValue", new BsonRegularExpression(pTagValue, "i"));
            
            return await Store.FindAllAsync(filter).ConfigureAwait(false);
        }        

        public TagManager(AbstractStore<Tag> store) : base(store)
        {
        }
    }
}