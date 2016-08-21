using System;
using System.Collections.Generic;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate;
using NHibernate.Linq;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public class GroupDao : IGroupDAL
    {
        public GroupDao()
        {
            Users = new HashSet<UserDao>();
        }

        public virtual int Id { get; protected set; }
        public virtual string GroupName { get; set; }
        public virtual EmailAddressDao GroupAddress { get; set; }
        public virtual ISet<UserDao> Users { get; protected set; }

        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        public virtual void AddUser(UserDao user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (!Users.Contains(user))
            {
                user.AddGroup(this);
                Users.Add(user);
            }
            //if looking for code wich save this changes take a look at UserDAO.AddUserToGroup() method. it's there.
        }

        public virtual void AddUsers(ISet<UserDao> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException("users");
            }
            foreach (var user in users)
            {
                if (!Users.Contains(user))
                {
                    user.AddGroup(this);
                    Users.Add(user);
                }
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        public virtual void RemoveUser(UserDao user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (Users.Contains(user))
            {
                Users.Remove(user);
                user.RemoveFrom(this);
            }
        }

        public static GroupDao GetGroupById(ISession session, int groupId)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            return session.Get<GroupDao>(groupId);
        }

        public virtual GroupDao FindGroupByName(ISession session, string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException("groupName");
            }

            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            return session.Query<GroupDao>().FirstOrDefault(x => x.GroupName == groupName);
        }

        public virtual GroupEntity FindGroupByName(string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException("groupName");
            }

            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var groupDao = FindGroupByName(uow.Session, groupName);
                return GroupEntity.ToBusinessItem(groupDao);
            }
        }

        /// <summary>
        /// Result will be null if group with same name already exist.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public virtual GroupEntity RegisterNewGroup(string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException("groupName");
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var ownerGroup = FindGroupByName(uow.Session, groupName);
                if (ownerGroup == null)
                {
                    ownerGroup = new GroupDao
                    {
                        GroupName = groupName
                    };
                    uow.Session.Save(ownerGroup);
                    uow.TransactionalFlush();
                    return GroupEntity.ToBusinessItem(ownerGroup);
                }
                return null;
            }
        }
    }
}