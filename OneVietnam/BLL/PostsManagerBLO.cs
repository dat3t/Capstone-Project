using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public partial class ApplicationUserManager
    {
        public virtual async Task<IdentityResult> AddPostAsync(string userId, Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }
            await _userStore.AddPostAsync(user, post).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<List<Post>> GetPostsAsync(string userId)
        {
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }
            return _userStore.GetPostsAsync(user);
        }

        public Post GetPostByIdAsync(string pUserId, string pPostId)
        {

            var posts = GetPostsAsync(pUserId);
            if (posts != null)
            {
                foreach (var post in posts.Result)
                {
                    if (pPostId.Equals(post.Id))
                    {
                        return post;
                    }
                }                
            }
            return null;
        }

        public  Post GetPostByIdAsync(string pPostId)
        {
            var user =  _userStore.FindUserByPostIdAsync(pPostId); ;
            if(user.Result != null)
            {
                return GetPostByIdAsync(user.Result[0].Id, pPostId);
            }
            return null;
        }

        public async Task<IdentityResult> UpdatePostAsync(string pUserId, Post pPost)
        {
            if (pPost == null)
                throw new ArgumentNullException(nameof(pPost));
            var user = await FindByIdAsync(pUserId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }
            await _userStore.UpdatePostAsync(user, pPost).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<IdentityResult> DeletePostAsync(string pUserId, string pPostId)
        {
            Post post = GetPostByIdAsync(pUserId, pPostId);
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            var user = await FindByIdAsync(pUserId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }
            await _userStore.DeletePostAsync(user, post).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

    }
}