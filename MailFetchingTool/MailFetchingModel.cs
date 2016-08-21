using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MailFetchingTool.Service_References.ServiceReference1;

namespace MailFetchingTool
{
    public sealed class MailFetchingModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private MailProtocol protocol;

        public IEnumerable<EnumHelper.ValueDescription> ProtocolList
        {
            get { return EnumHelper.GetAllValuesAndDescriptions<MailProtocol>(); }
        }

        public MailProtocol Protocol
        {
            get { return protocol; }
            set
            {
                if (protocol != value)
                {
                    protocol = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Port":
                        int a;
                        Error = !int.TryParse( Port, out a )
                            ? "Port cannot be empty and has to be numeric."
                            : ( a < 0 || a >= ushort.MaxValue )
                                ? "Port should be in range from 0 to 65535."
                                : null;
                        break;

                    case "Host":
                        Error = string.IsNullOrWhiteSpace( Host )
                            ? "Host cannot be empty."
                            : null;
                        break;

                    case "Username":
                        Error = string.IsNullOrWhiteSpace( Username )
                            ? "Username cannot be empty."
                            : null;
                        break;
                }
                return Error;
            }
        }

        public string Error { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}