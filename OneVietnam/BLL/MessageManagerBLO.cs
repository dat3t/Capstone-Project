using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MongoDB.Driver;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class MessageManager:AbstractManager<Message>
    {
        public MessageManager(AbstractStore<Message> store) : base(store)
        {
        }
        public static MessageManager Create(IdentityFactoryOptions<MessageManager> options,
            IOwinContext context)
        {
            var manager =
                new MessageManager(new MessageStore(context.Get<ApplicationIdentityContext>().Messages));
            return manager;
        }
        public async Task<List<Message>> FindAllMessagesByUserIdAsync(string senderId)
        {
            var builder = Builders<Message>.Filter;
            var filter = builder.Eq("SenderId", senderId) & builder.Eq("DeletedFlag", false);
            var sort = Builders<Message>.Sort.Descending("CreatedDate");
            return await Store.FindAllAsync(filter, sort).ConfigureAwait(false);
        }

        public async Task<List<Message>> FindAllMessagesByUserIdAsync(string senderId,BaseFilter baseFilter)
        {
            if (baseFilter.IsNeedPaging)
            {
                var builder = Builders<Message>.Filter;
                var filter = builder.Eq("SenderId", senderId) & builder.Eq("DeletedFlag", false);
                var sort = Builders<Message>.Sort.Descending("CreatedDate");
                return await Store.FindAllAsync(baseFilter, filter, sort).ConfigureAwait(false);
            }
            else
            {
                return await FindAllMessagesByUserIdAsync(senderId);
            }
        }        
    }
}