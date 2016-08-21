using System;
using System.Windows;
using MailBot.Client.ErrorHandling;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.Windows.Dialogues;
using MailBot.Client.Windows.Main;

namespace MailBot.Client
{
    public partial class App
    {
        private UserProxy currentUser;

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += ErrorHandler.OnUnhandledException;
        }

        public UserProxy CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (value != null)
                {
                    currentUser = value;
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                }
            }
        }

        public new static App Current
        {
            get { return Application.Current as App; }
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            AuthorizationRoutine();
        }

        public void AuthorizationRoutine()
        {
            var authWindow = new AuthenticationWindow();
            authWindow.Show();
            MainWindow = authWindow;
        }

        public void RegistrationRoutine()
        {
            var authWindow = new RegistrationWindow();
            authWindow.Show();
            MainWindow = authWindow;
        }
    }
}