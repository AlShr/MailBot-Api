using System;
using System.Windows;
using MailBot.Client.ErrorHandling;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.ServiceCalls;
using MailBot.Client.Windows.Dialogues;

namespace MailBot.Client.Windows.Settings
{
    public partial class MailboxStatusWindow
    {
        private readonly EmailAddressProxy proxy;

        private MailboxStatusWindow(EmailAddressProxy proxy)
        {
            InitializeComponent();
            this.proxy = proxy;
            emailTextblock.Text = proxy.Address;
            statusTextblock.Text = proxy.VerificationKey.Status;
            if (proxy.VerificationKey.Status != "true")
            {
                gridInput.Visibility = Visibility.Collapsed;
                gridVerify.Visibility = Visibility.Collapsed;
                flipButton.Visibility = Visibility.Collapsed;
            }
        }

        public static MailboxStatusWindow Create(EmailAddressProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException( "proxy" );
            }
            return new MailboxStatusWindow( proxy );
        }

        private void SendVerificationMailButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceCaller.SendVerificationCode( proxy.Address );
                StatementWindow.Show( "Success!", "Verification mail has been sent.", "Check your mailbox for verification email.", this );
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowMessage( ex );
            }
        }

        private void FlipButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (gridInput.Visibility == Visibility.Visible)
            {
                gridInput.Visibility = Visibility.Collapsed;
                gridVerify.Visibility = Visibility.Visible;
            }
            else
            {
                gridInput.Visibility = Visibility.Visible;
                gridVerify.Visibility = Visibility.Collapsed;
            }
        }
    }
}