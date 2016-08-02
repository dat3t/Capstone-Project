using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Driver;
using Ninject.Activation;
using OneVietnam.Common;
using OneVietnam.DAL;
using OneVietnam.DTL;
using OneVietnam.Models;
using Tag = OneVietnam.DTL.Tag;

namespace OneVietnam.BLL
{
    public partial class PostManager : AbstractManager<Post>
    {

        public static PostManager Create(IdentityFactoryOptions<PostManager> options,
            IOwinContext context)
        {
            var manager =
                new PostManager(new PostStore(context.Get<ApplicationIdentityContext>().Posts));
            return manager;
        }   
           
        //OK
        public async Task<List<Post>> FindByUserId(string userId)
        {
            var filter = Builders<Post>.Filter.Eq("UserId", userId);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return await Store.FindAllAsync(filter).ConfigureAwait(false);
        }
        //OK                                
        public async Task<List<BsonDocument>> FullTextSearch(string query, BaseFilter filter)
        {
            var sort = Builders<Post>.Sort.MetaTextScore("TextMatchScore").Ascending("CreatedDate");
            return await Store.FullTextSearch(query, filter, sort).ConfigureAwait(false);
        }             
        public PostManager(PostStore store) : base(store)
        {
        }


        public async Task<List<Illustration>> GetIllustration(HttpFileCollectionBase pFiles, string pBlobContainerName)
        {
            try
            {
                int fileCount = pFiles.Count;
                if (fileCount > 0 && !string.IsNullOrEmpty(pFiles[0]?.FileName))
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer blobContainer = blobClient.GetContainerReference(pBlobContainerName);
                    await blobContainer.DeleteIfExistsAsync();
                    await blobContainer.CreateAsync();
                    await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                    for (int i = 0; i < fileCount; i++)
                    {
                        if (pFiles[i] != null)
                        {
                            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(pFiles[i].FileName));
                            var inputstr = pFiles[i].InputStream;
                         await   blob.UploadFromStreamAsync(inputstr);
                        }                        
                    }
                    var blobList = blobContainer.ListBlobs();
                    List<Illustration> illList = new List<Illustration>();
                    foreach (var blob in blobList)
                    {
                        Illustration newIll = new Illustration(blob.Uri.ToString());
                        illList.Add(newIll);
                    }
                    return illList;
                }
            }
            catch (Exception ex)
            {                
                return null;
            }
            return null;
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        public async Task<List<Tag>> AddAndGetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if (pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (!string.IsNullOrEmpty(addedTagValueList))
                {
                    List<Tag> newList = new List<Tag>();
                    var tagsInDb = await pTagManager.GetTagsValueAsync();
                    int numberTags = 0;
                    if (tagsInDb != null)
                    {
                        numberTags = tagsInDb.Count;
                    }

                    foreach (var tag in addedTagValueList.Split(','))
                    {
                        if (tagsInDb == null | (tagsInDb != null && !tagsInDb.Contains(tag)))
                        {
                            Tag newTag = new Tag(string.Concat("Tag_", numberTags.ToString()), tag);
                            await pTagManager.CreateAsync(newTag);
                            numberTags = numberTags + 1;
                            newList.Add(newTag);
                        }
                        else
                        {
                            var existTag = await pTagManager.FindTagByValueAsync(tag);
                            newList.Add(existTag[0]);
                        }
                    }
                    return newList;
                }
                return null;
            }
            return null;
        }

        public async Task<List<Post>> FindAllDescenderAsync(BaseFilter basefilter)
        {
            var filter = Builders<Post>.Filter.Eq("DeletedFlag", false);
            var sort = Builders<Post>.Sort.Descending("CreatedDate");
            return await Store.FindAllAsync(basefilter, filter, sort);
        }

        public async Task<List<Post>> SearchPostMultipleQuery(string title, DateTimeOffset? createdDateFrom, DateTimeOffset? createdDateTo, bool? status)
        {
            var builder = Builders<Post>.Filter;
            FilterDefinition<Post> filter = null;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var textFilter = builder.Regex("Title", new BsonRegularExpression(title, "i"));
                filter = textFilter;
            }
            if (createdDateFrom != null)
            {
                var dateFrpm = builder.Gte("CreatedDate", createdDateFrom);
                if (filter == null)
                {
                    filter = dateFrpm;
                }
                else
                {
                    filter = filter & dateFrpm;
                }
            }

            if (createdDateTo != null)
            {
                var dateTo = builder.Lt("CreatedDate", createdDateTo);
                if (filter == null)
                {
                    filter = dateTo;
                }
                else
                {
                    filter = filter & dateTo;
                }
            }

            if (status != null)
            {
                var statusFilter = builder.Eq("Status", status);
                if (filter == null)
                {
                    filter = statusFilter;
                }
                else
                {
                    filter = filter & statusFilter;
                }
            }
            return await Store.FindAllAsync(filter);
        }

    }
}