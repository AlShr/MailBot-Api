using System;

namespace MailBot.Service.MailFetching.Arguments
{
    public sealed class HostInfo
    {
        /// <exception cref="ArgumentException"><paramref name="port" /> is out of [0,65535] range.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hostname" /> is <see langword="null" />.</exception>
        public HostInfo(MailProtocol protocol, string hostname, int port, bool usingSsl)
        {
            if (string.IsNullOrWhiteSpace( hostname ))
            {
                throw new ArgumentNullException( "hostname" );
            }
            if (port < 0 || port >= ushort.MaxValue)
            {
                throw new ArgumentException( "port" );
            }
            Protocol = protocol;
            Hostname = hostname;
            Port = port;
            UsingSsl = usingSsl;
        }

        public MailProtocol Protocol { get; private set; }
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public bool UsingSsl { get; private set; }
    }
}