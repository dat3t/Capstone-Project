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
    }
}