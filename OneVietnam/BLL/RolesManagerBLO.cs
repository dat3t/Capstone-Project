using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;

namespace OneVietnam.BLL
{
    public partial class ApplicationRoleManager
    {
        public async Task<List<IdentityRole>> AllRolesAsync()
        {
            return await _roleStore.AllRolesAsync();
        }
    }
}