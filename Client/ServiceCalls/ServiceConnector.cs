using System;
using System.Net;
using MailBot.Client.MailBotServiceReference;

namespace MailBot.Client.ServiceCalls
{
    internal static class ServiceConnector
    {
        public static void ServiceCall(Action<MailBotServiceClient> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException( "action" );
            }
            using (var client = GetServiceClient())
            {
                action( client );
            }
        }

        private static MailBotServiceClient GetServiceClient()
        {
            try
            {
                return new MailBotServiceClient();
            }
            catch (Exception e)
            {
                throw new WebException( "Could not connect to service.", e );
            }
        }
    }
}