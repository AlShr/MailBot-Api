using System;
using System.Windows;

namespace MailBot.Client.Windows.Dialogues
{
    /// <summary>
    ///     Interaction logic for YesNoDialogue.xaml
    /// </summary>
    public partial class YesNoDialogue
    {
        private readonly Action no;
        private readonly Action yes;

        private YesNoDialogue(string title, string header, string message, Action yesCallback, Action noCallback)
        {
            InitializeComponent();
            Title = title;
            accentedTextBlock.Text = header;
            messageTextBlock.Text = message;
            yes = yesCallback;
            no = noCallback;
        }

        public static void Ask(string title, string header, string message, Action yesCallback, Action noCallback, Window parent = null)
        {
            var dialogue = new YesNoDialogue( title, header, message, yesCallback, noCallback ) { Owner = parent };
            dialogue.Show();
        }

        private void OnYes(object sender, RoutedEventArgs e)
        {
            if (yes != null)
            {
                yes();
            }
            Close();
        }

        private void OnNo(object sender, RoutedEventArgs e)
        {
            if (no != null)
            {
                no();
            }
            Close();
        }

        private void YesNoDialogue_OnClosed(object sender, EventArgs e)
        {
            OnNo( sender, null );
        }
    }
}