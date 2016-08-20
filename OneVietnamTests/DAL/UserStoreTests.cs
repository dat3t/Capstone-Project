using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneVietnam.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneVietnam.DAL.Tests
{
    [TestClass()]
    public class UserStoreTests
    {
        [TestMethod()]        
        public void UserStoreTest()
        {
            "1234567890".Should().Be("1234567890");
        }

        [TestMethod()]
        public void AllUsersAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindUsersByRoleAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TextSearchUsersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TextSearchUsersTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TextSearchUsersTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TextSearchMultipleQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateOneByFilterAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PushAdminNotificationToAllUsersAsyncTest()
        {
            Assert.Fail();
        }
    }
}