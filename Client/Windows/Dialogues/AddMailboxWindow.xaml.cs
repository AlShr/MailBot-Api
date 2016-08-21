using System;
using System.ComponentModel;
using System.Windows;
using MailBot.Client.ServiceCalls;
using MailBot.Client.Validation;

namespace MailBot.Client.Windows.Dialogues
{
    public partial class AddMailboxWindow
    {
        //private bool asking;

        public AddMailboxWindow()
        {
            InitializeComponent();
        }

        private void AddBoxClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace( emailBox.Text ))
            {
                StatementWindow.ShowError( "Error.", "Error.", "Please fill email textbox before trying to query it.", this );
                return;
            }
            var mailbox = emailBox.Text;
            try
            {
                UserInputValidator.AssertMailBox( mailbox );
            }
            catch (Exception validationException)
            {
                StatementWindow.ShowError( "Error.", "Error.", validationException.Message );
                return;
            }
            try
            {
                var proxy = ServiceCaller.AddMailBox( mailbox );
                StatementWindow.Show( "Success.", "Operation successful!", string.Format( "Mailbox {0} successfully added.", proxy.Address ), this );
            }
            catch
            {
                StatementWindow.ShowError( "Failure.", "Operation not successful!", "Couldn't add this mailbox. Possible reason is it exists already.", this );
            }
        }

        /*
        private void VerifyButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace( tokenBox.Text ))
            {
                StatementWindow.ShowError( "Error.", "Error.", "Please fill token textbox before trying to verify it.", this );
                return;
            }
            var token = tokenBox.Text;
            if (ServiceCaller.SendVerificationCode( vkProxy.EmailAdress.Address, token ))
            {
                StatementWindow.Show( "Success.", "Operation successful!", string.Format( "You have successfully linked your {0} mailbox to your account!", vkProxy.EmailAdress.Address ), this );
            }
            else
            {
                StatementWindow.ShowError( "Failure.", "Operation not successful!", "Verification key doesn't appear to be valid.", this );
            }
        }*/

        private void AddMailboxWindow_OnClosing(object sender, CancelEventArgs e)
        {
            /*
            if (!asking && !string.IsNullOrWhiteSpace(emailBox.Text))
            {
                e.Cancel = true;
                YesNoDialogue.Ask( "Close?", "Really close?", "You will lose an opportunity to verify inserted email if you close now.", YesCallback, null, this );
            }*/
        }
    }
}