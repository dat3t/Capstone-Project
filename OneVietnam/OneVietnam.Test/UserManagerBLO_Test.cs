using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NUnit.Framework;
using OneVietnam.BLL;
using OneVietnam.DAL;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using MongoDB.Driver.Core.Connections;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    [TestFixture]
    public class UserManagerBLO_Test
    {
        private ApplicationUserManager userManager;
        [SetUp]
        public void Setup()
        {
            var context = ApplicationIdentityContext.Create();
            userManager = new ApplicationUserManager(new UserStore(context.Users));
        }
        [Test]
        public async Task GetAvatarByIdAsync()
        {
            var avatar_test = "https://scontent.xx.fbcdn.net/t31.0-1/p960x960/11084158_813722535349851_647901446651038838_o.jpg";
            var userId_test = "57ae28822af6170ec006cde1";
            var avatar_result = await userManager.GetAvatarByIdAsync(userId_test);
            Assert.AreEqual(avatar_test,avatar_result);
        }
        [Test]
        public async Task AddUserToRolesAsync()
        {
            string[] roles = {"Admin"};
            var userId = "57b5d7d92af61928ace5e01e";
            var result = await userManager.AddUserToRolesAsync(userId,roles);
            Assert.AreEqual(true, result.Succeeded);
            var user = await userManager.FindByIdAsync(userId);
            var userRoles = user.Roles;
            CollectionAssert.Contains(userRoles,"Admin");
            //Assert.
        }
        [Test]
        public async Task AllUserAsync()
        {
            var list = await userManager.AllUsersAsync();
            int count = 4;
            Assert.AreEqual(count,list.Count);
        }

        [Test]
        public async Task RemoveUserFromRolesAsync()
        {
            var userId = "57b5d7d92af61928ace5e01e";
            string[] roles = { "Admin" };
            var result = await userManager.RemoveUserFromRolesAsync(userId, roles);
            Assert.AreEqual(true,result.Succeeded);
            var user = await userManager.FindByIdAsync(userId);
            var userRoles = user.Roles;
            CollectionAssert.DoesNotContain(userRoles,"Admin");
        }
        [Test]
        public async Task SetEmailConfirmed()
        {
            var userId = "57ae28822af6170ec006cde1";
            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.SetEmailConfirmed(user);
            Assert.AreEqual(true,result.Succeeded);
            Assert.AreEqual(true,user.EmailConfirmed);
        }
        [Test]
        public async Task FindUsersByRoleAsync()
        {
            var role = new IdentityRole("Admin");
            var list = await userManager.FindUsersByRoleAsync(role);
            int expectedAdminCount = 1;
            Assert.AreEqual(expectedAdminCount,list.Count);
        }
        [Test]
        public async Task TextSearchUsers()
        {
            var filter = new BaseFilter();
            filter.IsNeedPaging = false;
            var query = "Tai";
            var list = await userManager.TextSearchUsers(filter, query);
            var expectedCount = 2;
            Assert.AreEqual(expectedCount,list.Count);
        }
        [Test]
        public async Task GetUserNameByIdAsync()
        {
            var id = "57ae28822af6170ec006cde1";
            var userName = await userManager.GetUserNameByIdAsync(id);
            var expected = "MinhTai Le";
            Assert.AreEqual(expected,userName);
        }
        [Test]
        public async Task AddConnection()
        {
            var userId = "57b5d7d92af61928ace5e01e";
            var user = await userManager.FindByIdAsync(userId);
            var count = 0;
            if (user.Connections != null) count = user.Connections.Count;
            var connection = new Connection
            {
                ConnectionId = @"dcb2f885-bcc9-462f-b6d9-6c2b614bffb8",
                UserAgent =
                    @"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36",
                Connected = true
            };
            await userManager.AddConnection(userId, connection);
            var actualUser = await userManager.FindByIdAsync(userId);
            var actualCount = 0;
            if (actualUser.Connections != null) actualCount = actualUser.Connections.Count;
            Assert.AreEqual(count+1, actualCount);
        }
        [Test]
        public async Task AddMessage()
        {
            var userId = "57b353be2af6172e70f2e20c";
            var friendId = "57b5d7d92af61928ace5e01e";
            var content = "Xin Chao";
            await userManager.AddMessage(userId, friendId, content);
            var user = await userManager.FindByIdAsync(userId);
            var friend = await userManager.FindByIdAsync(friendId);
            Assert.AreEqual(content,user.Conversations[friendId].LastestMessage);
            Assert.AreEqual(content, friend.Conversations[userId].LastestMessage);

        }
        
    }
}