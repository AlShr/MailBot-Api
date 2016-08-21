using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MailBot.Client.ControlHelpers;
using MailBot.Client.ErrorHandling;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.ServiceCalls;

namespace MailBot.Client.Windows.Dialogues
{
    public partial class AuthenticationWindow
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
            Worker.DoWork += SignInBackground;
            UiDisabler = new UiDisabler( ( (Visual) Content ).GetChildren().OfType<FrameworkElement>() );
        }

        ~AuthenticationWindow()
        {
            Worker.DoWork -= SignInBackground;
        }

        private void SignInBackground(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var credentials = (Tuple<string, string>) doWorkEventArgs.Argument;
            if (!string.IsNullOrWhiteSpace( credentials.Item1 ) && !string.IsNullOrWhiteSpace( credentials.Item2 ))
            {
                DisableUi();
                try
                {
                    var userProxy = ServiceCaller.Authenticate( credentials.Item1, credentials.Item2 );
                    userProxy.Password = credentials.Item2;
                    OpenMainWindow( userProxy );
                }
                catch (Exception authenticationError)
                {
                    ErrorHandler.ShowMessage( authenticationError );
                }
                RestoreUi();
            }
        }

        private void ButtonLogIn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Worker.IsBusy)
            {
                var credentials = new Tuple<string, string>( loginBox.Text, passwordBox.Password );
                Worker.RunWorkerAsync( credentials );
            }
        }

        private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.RegistrationRoutine();
            Close();
        }

        private void ResetUsernameValidation(object sender, EventArgs e)
        {
            ControlValidator.PaintGood( (Control) sender );
        }

        private void OpenMainWindow(UserProxy user)
        {
            InvokeDispatcher( () =>
            {
                App.Current.CurrentUser = user;
                Close();
            } );
        }

        private void DisableUi()
        {
            InvokeDispatcher( () =>
            {
                UiDisabler.Disable();
                progressRingSignup.IsActive = true;
            } );
        }

        private void RestoreUi()
        {
            InvokeDispatcher( () =>
            {
                UiDisabler.Enable();
                progressRingSignup.IsActive = false;
            } );
        }

        private void ClearFields()
        {
            InvokeDispatcher( () =>
            {
                ControlValidator.PaintGood( loginBox );
                ControlValidator.PaintGood( passwordBox );
                loginBox.Clear();
                passwordBox.Clear();
            } );
        }
    }
}