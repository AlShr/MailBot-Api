using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MailBot.Client.Windows.Dialogues
{
    public partial class StatementWindow
    {
        private StatementWindow(string title, string accentedMessage, string message, StatementType statementType)
        {
            InitializeComponent();
            Title = title;
            accentedTextBlock.Text = accentedMessage;
            messageTextBlock.Text = message;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
            switch (statementType)
            {
                case StatementType.Error:
                    pictureRect.OpacityMask = new VisualBrush( App.Current.Resources["appbar_bug"] as Visual );
                    break;
                case StatementType.Ok:
                    pictureRect.OpacityMask = new VisualBrush( App.Current.Resources["appbar_check"] as Visual );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void ShowError(string windowTitle, string accentedMessage, string message, Window parent = null)
        {
            App.Current.Dispatcher.Invoke( () =>
            {
                var dialog = new StatementWindow( windowTitle, accentedMessage, message, StatementType.Error )
                {
                    Owner = parent
                };
                dialog.Show();
            }, DispatcherPriority.Background );
        }

        public static void Show(string windowTitle, string accentedMessage, string message, Window parent = null)
        {
            App.Current.Dispatcher.Invoke( () =>
            {
                var dialog = new StatementWindow( windowTitle, accentedMessage, message, StatementType.Ok )
                {
                    Owner = parent
                };
                dialog.Show();
            }, DispatcherPriority.Background );
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    internal enum StatementType
    {
        Error,
        Ok
    }
}