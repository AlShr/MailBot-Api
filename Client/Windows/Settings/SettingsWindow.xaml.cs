using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MailBot.Client.ObjectRepo;
using MailBot.Client.ServiceCalls;
using MailBot.Client.Windows.Dialogues;
using MailBot.Client.Windows.Settings.Mailboxes;

namespace MailBot.Client.Windows.Settings
{
    public partial class SettingsWindow
    {
        private FrameworkElement activeControl;
        private Dictionary<FrameworkElement, FrameworkElement> dictionary;

        public SettingsWindow()
        {
            InitializeComponent();
            userSettingsName.Text += App.Current.CurrentUser.Login + ".";
            InitializeControlMap();
            ReloadUserMailboxes();
        }

        private FrameworkElement ActiveControl
        {
            get { return activeControl; }
            set
            {
                if (activeControl != null)
                {
                    activeControl.Visibility = Visibility.Collapsed;
                }
                if (value != null)
                {
                    activeControl = value;
                    activeControl.Visibility = Visibility.Visible;
                }
            }
        }

        private void InitializeControlMap()
        {
            dictionary = new Dictionary<FrameworkElement, FrameworkElement>
            {
                { listItemUserStatus, gridUserSettings },
                { listItemMailBoxes, gridMailboxes }
            };
        }

        private void ReloadUserMailboxes()
        {
            //var list = Generator.GenerateMailboxes( 100 );
            var list = ServiceCaller.GetUserMailboxes(App.Current.CurrentUser);
            MailboxDisplayer.DisplayInto( listView, list );
        }

        private void ListViewItemClickHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var element = sender as FrameworkElement;
            if (element != null && dictionary.ContainsKey( element ))
            {
                ActiveControl = dictionary[element];
            }
        }

        private void AddMailboxButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new AddMailboxWindow();
            window.Show();
        }
    }
}