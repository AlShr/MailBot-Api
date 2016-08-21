using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MailFetchingTool.Service_References.ServiceReference1;

//using SharedTypes;
//using SharedTypes.MailFetching;

namespace MailFetchingTool
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MailFetchingWindow : MetroWindow
    {
        private readonly BackgroundWorker backgroundMailFetcher;
        private readonly MailFetchingModel model;
        //private MailLoader m_mailClient;

        public MailFetchingWindow()
        {
            InitializeComponent();
            model = new MailFetchingModel
            {
                Host = "imap.gmail.com",
                Port = "993",
                Username = "somebotmail"
            };
            DataContext = model;
            backgroundMailFetcher = new BackgroundWorker();
            backgroundMailFetcher.DoWork += LoadMessages;
            backgroundMailFetcher.RunWorkerCompleted += OnBackgroundMailFetchingCompleted;
            backgroundMailFetcher.WorkerSupportsCancellation = true;
        }

        private void OnBackgroundMailFetchingCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            Dispatcher.InvokeAsync( ExitLoadingState, DispatcherPriority.Background );
            //m_mailClient.Dispose();
        }

        private void Button_Fetch_Click(object sender, RoutedEventArgs e)
        {
            if (!backgroundMailFetcher.IsBusy)
            {
                if (model.Error == null)
                {
                    buttonFetch.IsEnabled = false;
                    buttonFetch.Content = "Cancel";
                    try
                    {
                        /*
                        var hostInfo = new HostInfo( m_model.Protocol, m_model.Host, int.Parse( m_model.Port ), true );
                        var mailbox = new MailboxInfo( m_model.Username, TextboxPassword.Password );
                        m_mailClient = MailLoader.GetLoader( mailbox, hostInfo );
                         * */
                        backgroundMailFetcher.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show( ex.Message );
                        backgroundMailFetcher.CancelAsync();
                        buttonFetch.Content = "Fetch";
                    }
                    buttonFetch.IsEnabled = true;
                }
            }
            else
            {
                buttonFetch.IsEnabled = false;
                backgroundMailFetcher.CancelAsync();
                buttonFetch.Content = "Fetch";
                buttonFetch.IsEnabled = true;
            }
        }

        private void ExitLoadingState()
        {
            fetchingProgressRing.IsActive = false;
            fetchingProgressOverlay.Visibility = Visibility.Collapsed;
            messagesWrapPanel.Effect = null;
        }

        private void EnterLoadingState()
        {
            messagesWrapPanel.Effect = new BlurEffect { Radius = 10.0, KernelType = KernelType.Gaussian, RenderingBias = RenderingBias.Performance };
            fetchingProgressOverlay.Visibility = Visibility.Visible;
            fetchingProgressRing.IsActive = true;
        }

        private void ClearTiles()
        {
            messagesWrapPanel.Children.Clear();
        }

        private void LoadMessages(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var style = Application.Current.Resources["DefaultTileMargin"] as Style;
            Dispatcher.InvokeAsync( ClearTiles, DispatcherPriority.Background );
            Dispatcher.InvokeAsync( EnterLoadingState, DispatcherPriority.Background );

            var mbInfo = new MailboxInfoProxy
            {
                UserName = model.Username,
                Password = textboxPassword.Password
            };

            var hostInfo = new MailHostInfoProxy
            {
                Hostname = model.Host,
                Port = int.Parse( model.Port ),
                Protocol = model.Protocol,
                UsingSsl = true
            };

            using (var client = new BotMailServiceClient())
            {
                var messages = client.InitMessageReceive( mbInfo, hostInfo );
                var count = messages.Length;
                var currentMessage = 0;
                foreach (var message in messages)
                {
                    var localMessage = message;
                    var percentage = (int) ( ( currentMessage++ * 100f ) / count );
                    Dispatcher.InvokeAsync( () => { messagesWrapPanel.Children.Add( new Tile { Title = localMessage.Subject, Style = style, Tag = localMessage } ); }, DispatcherPriority.Background );
                    if (backgroundMailFetcher.CancellationPending)
                    {
                        break;
                    }
                }
            }
        }
    }
}