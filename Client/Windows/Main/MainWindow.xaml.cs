using System.Windows;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.Windows.Main.Mail;
using MailBot.Client.Windows.Settings;

namespace MailBot.Client.Windows.Main
{
    public partial class MainWindow
    {
        private readonly ConcreteMailPainter concreteMailPainter;
        private readonly MailListPainter mailListPainter;

        public MainWindow()
        {
            var currentUser = App.Current.CurrentUser;
            InitializeComponent();
            mailListPainter = new MailListPainter( threadStack );
            concreteMailPainter = new ConcreteMailPainter( concreteMailGrid );
            Title = string.Format( "{0} - {1}", currentUser.Login, "MailBot" );
        }

        public void DisplayConcreteMail(MailMessageProxy message)
        {
            concreteMailPainter.DisplayMail( message );
        }

        private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.AuthorizationRoutine();
            Close();
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            mailListPainter.LoadMails();
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Show();
        }
    }
}