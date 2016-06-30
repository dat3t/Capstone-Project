using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
namespace OneVietnam.DAL
{
    public class TagStore
    {
        private readonly IMongoCollection<DTL.Tag> _tags;

        public TagStore(IMongoCollection<DTL.Tag> pTags)
        {
            _tags = pTags;
        }

        public Task CreatAsync(DTL.Tag pTag)
        {
            return _tags.InsertOneAsync(pTag);
        }

        public virtual Task UpdateAsync(DTL.Tag pTag)
        {
            return _tags.ReplaceOneAsync(c => c.Id == pTag.Id, pTag, null);
        }

        public  Task<List<DTL.Tag>> GetTagsAsync()
        {
            return  _tags.Find(u => true).ToListAsync();
        }

        public List<string> GetTagsValueAsync()
        {
            var tagList =  _tags.Find(u => true).ToListAsync();
            if (tagList.Result.Count > 0)
            {
                List<string> tagValueList = new List<string>();
                foreach (var tagItem in tagList.Result)
                {
                    tagValueList.Add(tagItem.TagValue);
                }
                return tagValueList;
            }
            else
            {
                return null;
            }
            
        }

        public Task<List<DTL.Tag>> FindTagByValueAsync(string pTagValue)
        {
            return _tags.Find(u => u.TagValue.Contains(pTagValue)).ToListAsync();
        }
    }
}