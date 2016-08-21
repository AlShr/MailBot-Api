using System.Net.Mail;

namespace MailFetchingTool
{
    /// <summary>
    ///     Interaction logic for ConcreteMailViewer.xaml
    /// </summary>
    public partial class ConcreteMailViewer
    {
        private MailMessage message;

        public ConcreteMailViewer(MailMessage message)
        {
            InitializeComponent();
            this.message = message;
            DataContext = message;
        }
    }
}