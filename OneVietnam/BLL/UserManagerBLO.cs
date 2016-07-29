using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR.Hubs;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.Common;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public partial class ApplicationUserManager
    {        
        public async Task<List<ApplicationUser>> AllUsersAsync()
        {
            return await _userStore.AllUsersAsync();
        }
        
        /// <summary>
        /// Method to add user to multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> AddUserToRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(role => !userRoles.Contains(role)))
            {
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);
            }

            // Call update once when all roles are added
            return await UpdateAsync(user).ConfigureAwait(false);
        }
        /// <summary>
        /// Remove user from multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(userRoles.Contains))
            {
                await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
            }

            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> SetEmailConfirmed(ApplicationUser user)
        {
            var userEmailStore = (IUserEmailStore<ApplicationUser, string>)Store;
            //TODO            
            await userEmailStore.SetEmailConfirmedAsync(user, true);
            await UpdateSecurityStampAsync(user.Id);
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<List<ApplicationUser>> FindUsersByRoleAsync(IdentityRole role)
        {
            return await _userStore.FindUsersByRoleAsync(role);
        }

        public async Task<List<ApplicationUser>> TextSearchUsers(string query)
        {
            return await _userStore.TextSearchUsers(query);
        }

        public async Task<List<ApplicationUser>> TextSearchUsers(string query, BaseFilter baseFilter)
        {
            return await _userStore.TextSearchUsers(query, baseFilter);
        }
        public async Task<ICollection<Connection>> GetConnectionsById(string id)
        {

            var user = await _userStore.FindByIdAsync(id).ConfigureAwait(false);            
            return user.Connections;
        }
        public async Task AddConnection(string userId, Connection connection)
        {
            var user = await _userStore.FindByIdAsync(userId);
            if (user.Connections == null)
            {
                user.Connections = new List<Connection> {connection};
            }
            else
            {
                var conn = user.Connections.FirstOrDefault(c => c.ConnectionId == connection.ConnectionId);
                if (conn != null)
                {
                    conn.Connected = true;
                }
                else
                {
                    user.Connections.Add(connection);
                }
            }
            await _userStore.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task DisConnection(string userId, string connectionId)
        {
            var user = await _userStore.FindByIdAsync(userId);            
            var conn = user.Connections.FirstOrDefault(c => c.ConnectionId == connectionId);
            if (conn == null)
            {
                throw new Exception("Không tồn tại kết nối");
            }
            else
            {
                conn.Connected = false;
            }
            await _userStore.UpdateAsync(user);
        }

        public async Task AddMessage(string userId, string friendId, string content)
        {
            var sendMes = new Message(content,(int)MessageType.Send);
            var receiveMes = new Message(content,(int)MessageType.Receive);

            var user = await _userStore.FindByIdAsync(userId).ConfigureAwait(false);            
            // add message to sender user
            if(user.Conversations==null) user.Conversations = new SortedList<string, Conversation>();
            if (user.Conversations.ContainsKey(friendId))
            {
                user.Conversations[friendId].MessageList.Add(sendMes);                
            }
            else
            {
                var messages = new Conversation {MessageList = new List<Message> {sendMes}};
                user.Conversations.Add(friendId,messages);
            }
            user.Conversations[friendId].Seen = true;            

            var friend = await _userStore.FindByIdAsync(friendId).ConfigureAwait(false);

            if(friend.Conversations==null) friend.Conversations = new SortedList<string, Conversation>();
            if (friend.Conversations.ContainsKey(userId))
            {
                friend.Conversations[userId].MessageList.Add(receiveMes);
            }
            else
            {
                var messages = new Conversation { MessageList = new List<Message> { receiveMes } };
                friend.Conversations.Add(userId, messages);
            }
            friend.Conversations[userId].Seen = false;
            // add message to receiver user            
            // Update To Database     
            await _userStore.UpdateAsync(user).ConfigureAwait(false);
            await _userStore.UpdateAsync(friend).ConfigureAwait(false);
        }
    }
}