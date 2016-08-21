using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MailBot.Client.MailBotServiceReference;

namespace MailBot.Client.Windows.Main.Mail
{
    public class ConcreteMailPainter
    {
        private readonly Dispatcher dispatcher;
        private readonly Grid mailGrid;

        public ConcreteMailPainter(Grid mailGrid)
        {
            dispatcher = mailGrid.Dispatcher;
            this.mailGrid = mailGrid;
        }

        public void DisplayMail(MailMessageProxy mail)
        {
            dispatcher.InvokeAsync( () =>
            {
                mailGrid.Children.Clear();
                var grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength( 1, GridUnitType.Star ) },
                        new RowDefinition { Height = new GridLength( 5, GridUnitType.Star ) }
                    }
                };

                var infoPanel = new StackPanel();
                infoPanel.Children.Add( new TextBlock { Text = string.Format( "Sender: {0}", mail.Sender ) } );
                infoPanel.Children.Add( new TextBlock { Text = string.Format( "Subject: {0}", mail.Subject ) } );
                //infoPanel.Children.Add( new TextBlock { Text = string.Format( "Received: {0}", mail.ReceivedAt.ToString( "g" ) ) } );

                var bodyPanel = new TextBlock { Text = mail.Body };

                grid.Children.Add( infoPanel );
                Grid.SetRow( infoPanel, 0 );

                grid.Children.Add( bodyPanel );
                Grid.SetRow( bodyPanel, 1 );
                mailGrid.Children.Add( grid );
            } );
        }
    }
}