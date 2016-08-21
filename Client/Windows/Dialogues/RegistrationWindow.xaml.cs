using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MailBot.Client.ControlHelpers;
using MailBot.Client.ErrorHandling;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.ServiceCalls;
using MailBot.Client.Validation;

namespace MailBot.Client.Windows.Dialogues
{
    public partial class RegistrationWindow
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            Worker.DoWork += SignUpBackground;
            UiDisabler = new UiDisabler( ( (Visual) Content ).GetChildren().OfType<FrameworkElement>() );
        }

        ~RegistrationWindow()
        {
            Worker.DoWork -= SignUpBackground;
        }

        private void SignUpBackground(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var credentials = doWorkEventArgs.Argument as Tuple<string, string>;
            if (credentials != null && !string.IsNullOrWhiteSpace( credentials.Item1 ) && !string.IsNullOrWhiteSpace( credentials.Item2 ))
            {
                DisableUi();
                try
                {
                    var userProxy = ServiceCaller.Register( credentials.Item1, credentials.Item2 );
                    userProxy.Password = credentials.Item2;
                    OpenMainWindow( userProxy );
                }
                catch (Exception registrationError)
                {
                    ErrorHandler.ShowMessage( registrationError );
                }
                RestoreUi();
            }
        }

        private void OpenMainWindow(UserProxy user)
        {
            InvokeDispatcher( () =>
            {
                ( (App) Application.Current ).CurrentUser = user;
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

        private void ButtonSignUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Worker.IsBusy && UserInputIsValid())
            {
                var credentialsTuple = new Tuple<string, string>( loginBox.Text, passwordBox.Password );
                Worker.RunWorkerAsync( credentialsTuple );
            }
        }

        private bool UserInputIsValid()
        {
            var isValid = true;
            InvokeDispatcher( () =>
            {
                ControlValidator.AssertAndBlame( () => UserInputValidator.AssertUsernameRule( loginBox.Text ), loginBox, ref isValid );
                ControlValidator.AssertAndBlame( () => UserInputValidator.AssertPasswordRule( passwordBox.Password ), passwordBox, ref isValid );
                ControlValidator.AssertAndBlame( () => !passwordBox.Password.Equals( passwordBoxConfirm.Password ), "Passwords must match.", passwordBoxConfirm, ref isValid );
            } );
            return isValid;
        }

        private void ButtonBackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.AuthorizationRoutine();
            Close();
        }

        private void ClearFields()
        {
            InvokeDispatcher( () =>
            {
                passwordBox.Clear();
                passwordBoxConfirm.Clear();
                loginBox.Clear();
                ControlValidator.PaintGood( passwordBox );
                ControlValidator.PaintGood( passwordBoxConfirm );
                ControlValidator.PaintGood( loginBox );
            } );
        }
    }
}