using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using OneVietnam.DAL;

namespace OneVietnam.BLL
{
    [TestFixture]
    public class PostManagerBLO_Test
    {
        private PostManager postManager;
        [SetUp]
        public void Setup()
        {
            var context = ApplicationIdentityContext.Create();
            //userManager = new ApplicationUserManager(new UserStore(context.Users));
            postManager = new PostManager(new PostStore(context.Posts));
        }
        [Test]
        public async Task FindByUserId()
        {
            var userId = "57ae2ddc2af6190ec0aa0ec6";
            var listPost = await postManager.FindByUserId(userId);
            var expectedCount = 4;
            Assert.AreEqual(expectedCount,listPost.Count);
        }
        [Test]
        public async Task FindPostsByTypeAsync()
        {
            var baseFilter = new BaseFilter {IsNeedPaging = false};
            int type = 9;
            var list = await postManager.FindPostsByTypeAsync(baseFilter,type);
            int expectedCount = 5;
            Assert.AreEqual(expectedCount,list.Count);
        }
        [Test]
        public async Task FindPostByTypeAndUserIdAsync()
        {
            var baseFilter = new BaseFilter();
            baseFilter.IsNeedPaging = false;
            var userId = "57ae2ddc2af6190ec0aa0ec6";
            int type = 9;
            var list = await postManager.FindPostByTypeAndUserIdAsync(baseFilter, userId, type);
            int expectedCount = 4;
            Assert.AreEqual(expectedCount,list.Count);            
        }
        [Test]
        public async Task FindAllActiveAdminPostAsync()
        {
            var list = await postManager.FindAllActiveAdminPostAsync();
            var expectedCount = 5;
            Assert.AreEqual(expectedCount,list.Count);
        }
        [Test]
        public async Task FindAllDescenderByIdAsync()
        {
            var userId = "57ae2ddc2af6190ec0aa0ec6";
            var baseFilter = new BaseFilter();
            baseFilter.IsNeedPaging = false;            
            var list = await postManager.FindAllDescenderByIdAsync(baseFilter,userId);
            var expectedCount = 4;
            Assert.AreEqual(expectedCount,list.Count);
        }

    }
}