using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneVietnam.BLL;
using OneVietnam.DAL;

namespace UTOVN.TEST.Controller.PostController
{
    [TestClass]
    class CreatePost
    {
        private readonly OneVietnam.Controllers.PostController _postController;

        public CreatePost()
        {
            var _userStore = new UserStore();
            var  userManager = new ApplicationUserManager(_userStore);
            _postController = new OneVietnam.Controllers.PostController();
        }
    }
}
