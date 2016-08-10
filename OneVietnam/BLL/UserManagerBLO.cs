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
using OneVietnam.Models;

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

        public async Task<List<ApplicationUser>> TextSearchMultipleQuery(string userName, DateTimeOffset? createdDateFrom, DateTimeOffset? createdDateTo, string role, bool? isConnection)
        {
            var builder = Builders<ApplicationUser>.Filter;
            FilterDefinition<ApplicationUser> filter = null;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                var textFilter = builder.Regex("UserName", new BsonRegularExpression(userName, "i"));                
                filter = textFilter;
            }
            if (createdDateFrom != null)
            {                
                var dateFrpm = builder.Gte("CreatedDate", createdDateFrom);
                if (filter == null)
                {
                    filter = dateFrpm;
                }
                else
                {
                    filter = filter & dateFrpm;
                }
            }

            if (createdDateTo != null)
            {
                var dateTo = builder.Lt("CreatedDate", createdDateTo);
                if (filter == null)
                {
                    filter = dateTo;
                }
                else
                {
                    filter = filter & dateTo;
                }
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                var roleFilter = builder.Eq("Roles", role);
                if (filter == null)
                {
                    filter = roleFilter;
                }
                else
                {
                    filter = filter & roleFilter;
                }
            }

            if (isConnection != null)
            {
                var connectionFilter = builder.Eq("Connections.Connected", isConnection);
                if (filter == null)
                {
                    filter = connectionFilter;
                }
                else
                {
                    filter = filter & connectionFilter;
                }
            }
            return await _userStore.TextSearchMultipleQuery(filter);
        }

        public async Task<List<ApplicationUser>> TextSearchUsers(string query, BaseFilter baseFilter)
        {
            return await _userStore.TextSearchUsers(query, baseFilter);
        }
        //push admin notifications
        public async Task PushAdminNotificationToAllUserAsync(string adminId,Notification notification)
        {
            await _userStore.PushAdminNotificationToAllUserAsync(adminId, notification);
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
                if(conn==null) user.Connections.Add(connection);                
            }
            await _userStore.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task DisConnection(string userId, string connectionId)
        {
            var user = await _userStore.FindByIdAsync(userId);
            var stuffToRemove = user.Connections.SingleOrDefault(c => c.ConnectionId == connectionId);
            if (stuffToRemove!=null)
            {
                user.Connections.Remove(stuffToRemove);
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

        public async Task<bool> AddNotification(string id, NotificationViewModel model)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(id);
                if (user.Notifications == null)
                {
                    user.Notifications = new SortedList<string, Notification>();
                }
                var notification = new Notification(model);
                user.Notifications.Add(notification.Id, notification);
                await _userStore.UpdateAsync(user);
                return true;    
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task UpdateProfileUserAsync(UserProfileViewModel model)
        {            
            var builder = Builders<ApplicationUser>.Filter;
            var filter = builder.Eq("_id", new ObjectId(model.Id));
            var update =
                Builders<ApplicationUser>.Update.Set("UserName", model.UserName)
                    .Set("Gender", model.Gender)
                    .Set("DateOfBirth", model.DateOfBirth)
                    .Set("PhoneNumber", model.PhoneNumber)
                    .Set("Location", model.Location);
            await _userStore.UpdateOneByFilterAsync(filter, update);
        }
    }
}