using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using OneVietnam.DAL;
using OneVietnam.DTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneVietnam.BLL
{
    public partial class ApplicationUserManager
    {
        public virtual async Task<IdentityResult> AddLocationAsync(string userId, Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }
            await _userStore.AddLocationAsync(user, location).ConfigureAwait(false);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        //public virtual async Task<List<Post>> GetPostsAsync(string userId)
        //{
        //    var user = await FindByIdAsync(userId).ConfigureAwait(false);
        //    if (user == null)
        //    {
        //        throw new InvalidOperationException("Invalid user Id");
        //    }
        //    return _userStore.GetPostsAsync(user);
        //}
    }
}