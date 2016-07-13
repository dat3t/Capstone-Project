using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public partial class PostManager : IDisposable
    {
        private readonly PostStore _postStore;

        public PostManager(PostStore postStore)
        {
            _postStore = postStore;
        }
        public static PostManager Create(IdentityFactoryOptions<PostManager> options,
            IOwinContext context)
        {
            var manager =
                new PostManager(new PostStore(context.Get<ApplicationIdentityContext>().Posts));
            return manager;
        }
        public async Task CreatePostAsync(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            await _postStore.CreatePostAsync(post).ConfigureAwait(false);
            }
        //OK
        public async Task<List<Post>> FindByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return await _postStore.FindByUserIdAsync(userId).ConfigureAwait(false);
            }
        //OK
        public async Task<Post> FindById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            return await _postStore.FindByIdAsync(id).ConfigureAwait(false);

        }
        public async Task UpdatePostAsync(Post post)
                    {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            await _postStore.UpdateAsync(post).ConfigureAwait(false);
        }

        public async Task DeleteByIdAsync(string id)
            {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            await _postStore.DeleteByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<List<Post>> FindAllPostsAsync()
            {
            return await _postStore.FindAllPostAsync().ConfigureAwait(false);
        }
        public void Dispose()
            {
        }
    }
}