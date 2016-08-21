using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MailBot.Client.MailBotServiceReference;

namespace MailBot.Client.Windows.Settings.Mailboxes
{
    public static class MailboxDisplayer
    {
        public static void DisplayInto(
            ListView panel,
            IEnumerable<EmailAddressProxy> list)
        {
            ThreadUI( () =>
            {
                foreach (var boxItem in panel.Items.OfType<ListBoxItem>())
                {
                    boxItem.MouseUp -= ItemOnMouseUp;
                }
                panel.Items.Clear();
                var style = App.Current.Resources["MarginTextBlock"] as Style;
                foreach (var mailbox in list)
                {
                    var textBox = new TextBlock
                    {
                        Text = mailbox.Address,
                        Style = style
                    };
                    var item = new ListBoxItem
                    {
                        Content = textBox,
                        Tag = mailbox
                    };
                    item.MouseUp += ItemOnMouseUp;
                    panel.Items.Add( item );
                }
            } );
        }

        private static void ItemOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ThreadUI( () => MailboxStatusWindow.Create( ( sender as FrameworkElement ).Tag as EmailAddressProxy ) );
        }

        private static void ThreadUI(Action action)
        {
            App.Current.Dispatcher.InvokeAsync( action, DispatcherPriority.Background );
        }
    }
}