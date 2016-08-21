using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.Service.MailBotServiceDTO;
using MailBot.Service.MailFetching.Arguments;

namespace MailBot.Service.Converter
{
    public static class PolymorphConverter
    {
        #region Host Info converters

        public static HostInfoProxy ToProxyItem(HostInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new HostInfoProxy
            {
                Hostname = param.Hostname,
                Port = param.Port,
                Protocol = param.Protocol.ConvertEnum<MailProtocolProxy>(),
                UsingSsl = param.UsingSsl
            };
            return proxyItem;
        }

        public static HostInfo ToRepoItem(HostInfoProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var daoItem = new HostInfo(
                param.Protocol.ConvertEnum<MailProtocol>(),
                param.Hostname,
                param.Port,
                param.UsingSsl
                );
            return daoItem;
        }

        #endregion

        #region Mailbox Info converters

        public static MailboxInfoProxy ToProxyItem(MailboxInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new MailboxInfoProxy
            {
                Login = param.Login,
                Password = param.Password
            };
            return proxyItem;
        }

        public static MailboxInfo ToRepoItem(MailboxInfoProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var daoItem = new MailboxInfo(
                param.Login,
                param.Password
                );
            return daoItem;
        }

        #endregion

        #region Mail Message converters

        private static IEnumerable<AttachmentDao> GetMailAttachments(MailMessage mail)
        {
            var resultCollection = new List<AttachmentDao>();
            foreach (var attachment in mail.Attachments)
            {
                byte[] buf;
                using (var streamReader = new BinaryReader( attachment.ContentStream ))
                {
                    buf = streamReader.ReadBytes( (int) attachment.ContentStream.Length );
                }
                var bufferAttachmentDao = new AttachmentDao
                {
                    Contents = buf
                };
                resultCollection.Add( bufferAttachmentDao );
            }
            return resultCollection;
        }


        public static MailEntity ToBusinessItem(MailMessage param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }
            var mailEntity = new MailEntity
            {
                Subject = param.Subject,
                Body = param.Body
            };
            return mailEntity;
        }

        public static MailMessage ToBusinessItem(MailDao param)
        {
            //doesnt convert attchments, recipient and id fields
            if (param == null)
            {
                throw new ArgumentNullException();
            }
            var mailMessage = new MailMessage
            {
                Subject = param.Subject,
                Body = param.Body,
                Sender = new MailAddress( param.Sender.ToString() ),
                ReplyTo = null
            };
            return mailMessage;
        }

        public static MailMessageProxy ToRepoItem(MailDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }

            var proxyItem = new MailMessageProxy
            {
                Body = param.Body,
                Sender = ToProxyItem( param.Sender ),
                //todo: there is protected setter in MailProxy.Attachments. It should be? Can't convert attachments Dao -> Proxy
                //Attachments = param.Attachments,
                Subject = param.Subject
            };
            //todo:improve recipients convertation. remove "foreach"?
            foreach (var recipient in param.Recipients)
            {
                //proxyItem.Recipients.Add( ToProxyItem( recipient ) );
            }
            return proxyItem;
        }

        public static MailMessageProxy ToProxyItem(MailEntity param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }

            var recipients = new HashSet<EmailAddressProxy>();
            //todo: make this collection convertion not so much ugly.
            if (param.Recipients != null)
            {
                foreach (var email in param.Recipients)
                {
                    recipients.Add( ToProxyItem( email ) );
                }
            }

            var proxyItem = new MailMessageProxy
            {
                Body = param.Body,
                Sender = ToProxyItem( param.Sender ),
                Subject = param.Subject,
                Attachments = null,
                Recipients = recipients
            };
            return proxyItem;
        }

        #endregion

        #region User converters

        public static UserProxy ToProxyItem(UserDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new UserProxy
            {
                Id = param.Id,
                Login = param.Login,
                Password = param.Password
            };
            return proxyItem;
        }

        public static UserDao ToRepoItem(UserProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var daoItem = new UserDao
            {
                Login = param.Login,
                Password = param.Password
            };
            return daoItem;
        }

        public static UserProxy ToProxyItem(UserEntity param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new UserProxy
            {
                Id = param.Id,
                Login = param.Login,
                Password = param.Password
            };
            return proxyItem;
        }

        public static UserEntity ToBusinessItem(UserProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var buisnessItem = new UserEntity
            {
                Id = param.Id,
                Login = param.Login,
                Password = param.Password
            };
            return buisnessItem;
        }

        #endregion

        #region Group

        public static GroupProxy ToProxyItem(GroupEntity param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new GroupProxy
            {
                GroupName = param.GroupName,
                GroupAddress = param.GroupAddress == null ? null : ToProxyItem( param.GroupAddress )
            };
            return proxyItem;
        }

        public static GroupEntity ToBusinessItem(GroupProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var businessItem = new GroupEntity
            {
                GroupName = param.GroupName,
                GroupAddress = param.GroupAddress == null ? null : ToBusinessItem( param.GroupAddress )
            };
            return businessItem;
        }

        #endregion

        #region Verification messages converter

        public static VerificationKeyProxy ToProxyItem(VerificationKeyDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new VerificationKeyProxy
            {
                Id = param.Id,
                Key = param.Key,
                EmailAdress = ToProxyItem( param.EmailAddress ),
                Status = param.Status
            };
            return proxyItem;
        }

        public static VerificationKeyProxy ToProxyItem(VerificationKeyEntity param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var proxyItem = new VerificationKeyProxy
            {
                Id = param.Id,
                Key = param.Key,
                EmailAdress = ToProxyItem( param.EmailAddress ),
                Status = param.Status
            };
            return proxyItem;
        }

        public static VerificationKeyDao ToRepoItem(VerificationKeyProxy param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var daoItem = new VerificationKeyDao
            {
                Key = param.Key,
                EmailAddress = ToRepoItem( param.EmailAdress ),
                Status = param.Status
            };
            return daoItem;
        }

        #endregion

        #region Email address converter

        public static EmailAddressProxy ToProxyItem(EmailAddressDao param)
        {
            if (param == null)
            {
                throw new ArgumentException( "param" );
            }
            var proxyItem = new EmailAddressProxy
            {
                Id = param.Id,
                Address = param.Address,
                Owner = ToProxyItem( param.Owner ),
                VerificationKey = ToProxyItem( param.VerificationKey )
            };
            proxyItem.GroupOwner = new GroupProxy
            {
                GroupName = param.GroupOwner.GroupName,
                //GroupAddress = proxyItem
            };
            return proxyItem;
        }

        public static EmailAddressDao ToRepoItem(EmailAddressProxy param)
        {
            if (param == null)
            {
                throw new ArgumentException( "param" );
            }

            var daoItem = new EmailAddressDao
            {
                Address = param.Address,
                //Owner = ToRepoItem( param.Owner )
            };
            daoItem.GroupOwner = new GroupDao
            {
                //GroupAddress = daoItem,
                GroupName = param.GroupOwner.GroupName,
            };
            return daoItem;
        }

        public static EmailAddressProxy ToProxyItem(EmailAddressEntity param)
        {
            if (param == null)
            {
                throw new ArgumentException( "param" );
            }
            var verificationItem = new VerificationKeyProxy
            {
                Key = param.VerificationKey == null
                    ? null
                    : param.VerificationKey.Key,
                Status = param.VerificationKey == null
                    ? null
                    : param.VerificationKey.Status
            };
            var proxyItem = new EmailAddressProxy
            {
                Address = param.Address,
                Owner = ToProxyItem( param.Owner ),
                //GroupOwner = ToProxyItem(param.GroupOwner),
                VerificationKey = verificationItem
            };
            proxyItem.GroupOwner = new GroupProxy
            {
                //GroupAddress = proxyItem,
                GroupName = param.GroupOwner.GroupName,
                Id = param.GroupOwner.Id
            };
            return proxyItem;
        }

        public static EmailAddressEntity ToBusinessItem(EmailAddressProxy param)
        {
            if (param == null)
            {
                throw new ArgumentException( "param" );
            }
            var verificationItem = new VerificationKeyEntity
            {
                Key = param.VerificationKey == null
                    ? null
                    : param.VerificationKey.Key,
                Status = param.VerificationKey == null
                    ? null
                    : param.VerificationKey.Status
            };
            var proxyItem = new EmailAddressEntity
            {
                Address = param.Address,
                Owner = ToBusinessItem( param.Owner ),
                //GroupOwner = new GroupEntity
                VerificationKey = verificationItem
            };
            proxyItem.GroupOwner = new GroupEntity
            {
                //GroupAddress = proxyItem,
                GroupName = param.GroupOwner.GroupName
            };
            return proxyItem;
        }

        #endregion
    }
}