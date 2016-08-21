using System;
using MailBot.RegistryIntegrator;
using MailBot.Security;

namespace MailBot.ServiceConfigurator
{
    public static class Program
    {
        private static readonly string HelpMessage = string.Format(
            "===== MailBot Service Configurator ====={0}"
            + "This tool checks if MailBotService is configured for being hosted on THIS computer.{0}"
            + "This is achieved through writing important data to registry and checking if it will work.{0}"
            + "Necessary data includes bot account and password, data about imap and smtp of account.{0}"
            + "If you leave any of the fields empty, no change will be made.{0}"
            + "\tParameter -c enters configuration mode{0}"
            + "\tParameter -r removes existing configuration{0}"
            + "\t(c) GangOfFour 2015",
            Environment.NewLine );

        private static readonly string Usage = string.Format(
            "Usage: {0} -h | shows help{1}"
            + "       {0} -c | enters configuration mode{1}"
            + "       {0} -r | removes existing configuration",
            AppDomain.CurrentDomain.FriendlyName, Environment.NewLine );

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine( Usage );
            }
            else
            {
                switch (args[0])
                {
                    case "-h":
                        Console.WriteLine( HelpMessage );
                        break;
                    case "-c":
                        EnterConfigurationMode();
                        break;
                    case "-r":
                        RemoveExistingConfiguration();
                        break;
                    default:
                        Console.WriteLine( Usage );
                        break;
                }
            }
        }

        private static void RemoveExistingConfiguration()
        {
            RegistryServiceConfig.RemoveEntries();
            Console.WriteLine( "Existing configuration erased." );
        }

        private static void EnterConfigurationMode()
        {
            var name = InputParser.ParseOrReturn(
                "         Enter bot username: ", ParseType.Name, null );
            var password = InputParser.ParseOrReturn(
                "         Enter bot password: ", ParseType.Password, null );
            var imapHost = InputParser.ParseOrReturn(
                "            Enter IMAP host: ", ParseType.Host, null );
            var imapPort = InputParser.ParseOrReturn(
                "            Enter IMAP port: ", ParseType.Port, "Has to be integer between 0 and 65535." );
            var imapUsesSsl = InputParser.ParseOrReturn(
                "    Using SSL? (true/false): ", ParseType.Bool, "Has to be boolean (true/false)." );
            var smtpHost = InputParser.ParseOrReturn(
                "            Enter SMTP host: ", ParseType.Host, null );
            var smtpPort = InputParser.ParseOrReturn(
                "            Enter SMTP port: ", ParseType.Port, "Has to be integer between 0 and 65535." );
            var smtpUsesSsl = InputParser.ParseOrReturn(
                "    Using SSL? (true/false): ", ParseType.Bool, "Has to be boolean (true/false)." );
            Console.Write(
                "Entered data correct? (y/n): " );
            var correct = Console.ReadLine() == "y";

            if (correct)
            {
                Console.WriteLine( "Data correct. Attempting write..." );
                try
                {
                    Write( RegistryEntry.BoxName, name );
                    Write( RegistryEntry.BoxPassword, string.IsNullOrWhiteSpace( password )
                        ? password
                        : Encryptor.EncryptString( password ) );
                    Write( RegistryEntry.ImapHost, imapHost );
                    Write( RegistryEntry.ImapPort, imapPort );
                    Write( RegistryEntry.ImapSsl, imapUsesSsl );
                    Write( RegistryEntry.SmtpHost, smtpHost );
                    Write( RegistryEntry.SmtpPort, smtpPort );
                    Write( RegistryEntry.SmtpSsl, smtpUsesSsl );
                    Console.WriteLine( "Changes written." );
                }
                catch (Exception)
                {
                    Console.WriteLine( "Write failed." );
                    RegistryServiceConfig.RemoveEntries();
                    Console.WriteLine( "Rolled changes back." );
                }
            }
            else
            {
                Console.WriteLine( "Aborting..." );
            }
        }

        private static void Write(RegistryEntry entry, string value)
        {
            if (!string.IsNullOrWhiteSpace( value ))
            {
                RegistryServiceConfig.WriteEntry( entry, value );
            }
        }
    }
}