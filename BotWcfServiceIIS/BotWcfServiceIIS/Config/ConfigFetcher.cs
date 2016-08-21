using System;
using MailBot.RegistryIntegrator;
using MailBot.Security;
using MailBot.Service.MailBotServiceDTO;
using MailBot.Service.MailFetching;

namespace MailBot.Service.Config
{
    public static class ConfigFetcher
    {
        #region Fetching configuration from registry

        internal static void InitializeMailClients(MailboxInfoProxy box, HostInfoProxy imapInfo, HostInfoProxy smtpInfo)
        {
            try
            {
                BotMailbox.InitializeImap( box, imapInfo );
                BotMailbox.InitializeSMTP( box, smtpInfo );
            }
            catch (Exception exception)
            {
                throw new ApplicationException( "Failed while connecting to IMAP or SMTP. See inner exception for details.", exception );
            }
        }

        internal static void FetchConfiguration(out MailboxInfoProxy box, out HostInfoProxy imapInfo, out HostInfoProxy smtpInfo)
        {
            try
            {
                box = GetBoxInfo();
                imapInfo = GetImapInfo();
                smtpInfo = GetSmtpInfo();
            }
            catch (Exception exception)
            {
                throw new ApplicationException( "Failed while fetching configuration. You should've used ServiceConfigurator.exe before getting started.", exception );
            }
        }

        private static HostInfoProxy GetSmtpInfo()
        {
            var smtpHost = RegistryServiceConfig.ReadEntry( RegistryEntry.SmtpHost );
            var smtpPort = Convert.ToInt32( RegistryServiceConfig.ReadEntry( RegistryEntry.SmtpPort ) );
            var smtpSsl = Convert.ToBoolean( RegistryServiceConfig.ReadEntry( RegistryEntry.SmtpSsl ) );
            var smtpInfo = new HostInfoProxy { Hostname = smtpHost, Port = smtpPort, UsingSsl = smtpSsl };
            return smtpInfo;
        }

        private static HostInfoProxy GetImapInfo()
        {
            var imapHost = RegistryServiceConfig.ReadEntry( RegistryEntry.ImapHost );
            var imapPort = Convert.ToInt32( RegistryServiceConfig.ReadEntry( RegistryEntry.ImapPort ) );
            var imapSsl = Convert.ToBoolean( RegistryServiceConfig.ReadEntry( RegistryEntry.ImapSsl ) );
            var imapInfo = new HostInfoProxy { Hostname = imapHost, Port = imapPort, Protocol = MailProtocolProxy.Imap, UsingSsl = imapSsl };
            return imapInfo;
        }

        private static MailboxInfoProxy GetBoxInfo()
        {
            var boxName = RegistryServiceConfig.ReadEntry( RegistryEntry.BoxName );
            var boxPass = RegistryServiceConfig.ReadEntry( RegistryEntry.BoxPassword );
            boxPass = Encryptor.DecryptString( boxPass );
            var box = new MailboxInfoProxy { Login = boxName, Password = boxPass };
            return box;
        }

        #endregion
    }
}