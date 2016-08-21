using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MailBot.Client.ErrorHandling;
using MailBot.Client.MailBotServiceReference;
using MailBot.Client.ServiceCalls;

namespace MailBot.Client.Windows.Main.Mail
{
    public class MailListPainter : IDisposable
    {
        private readonly Dispatcher dispatcher;
        private readonly Panel panel;
        private readonly BackgroundWorker worker;

        public MailListPainter(Panel panel)
        {
            dispatcher = panel.Dispatcher;
            this.panel = panel;
            worker = new BackgroundWorker { WorkerReportsProgress = false };
            worker.DoWork += WorkerLoadMails;
        }

        public void LoadMails()
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        private void WorkerLoadMails(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            try
            {
                dispatcher.InvokeAsync( () => panel.Children.Clear() );
                ServiceCaller.GetMessagesForUser( App.Current.CurrentUser );
                var messages = ServiceCaller.GetMessagesForUser( App.Current.CurrentUser );
                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        var copiedMessage = message;
                        dispatcher.InvokeAsync( () =>
                        {
                            var gridStack = new StackPanel { Style = stackStyle, Focusable = false };
                            var grid = new Grid
                            {
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition { Width = new GridLength( 4, GridUnitType.Star ) },
                                    new ColumnDefinition { Width = new GridLength( 1.25, GridUnitType.Star ) }
                                },
                                MaxWidth = 300,
                                MinWidth = 300, //todo: constants is bad. extreme kostyl incoming
                                Tag = copiedMessage
                            };
                            grid.MouseLeftButtonDown += GridOnMouseLeftButtonDown;
                            var infoStack = GetMailStack();
                            infoStack.Children.Add( GetSenderBlock( copiedMessage.Sender.Address ) );
                            infoStack.Children.Add( GetSubjectBlock( copiedMessage.Subject ) );
                            infoStack.Children.Add( GetBodyBlock( copiedMessage.Body ) );
                            grid.Children.Add( infoStack );
                            Grid.SetColumn( infoStack, 0 );

                            var dateTimeStack = new StackPanel();
                            //var dateBlock = GetDateBlock( copiedMessage.ReceivedAt );
                            //var timeBlock = GetTimeBlock( copiedMessage.ReceivedAt );
                            //dateTimeStack.Children.Add( dateBlock );
                            //dateTimeStack.Children.Add( timeBlock );

                            grid.Children.Add( dateTimeStack ); //todo - correct date, requires changes in DAO
                            Grid.SetColumn( dateTimeStack, 1 );

                            gridStack.Children.Add( grid );
                            gridStack.Tag = copiedMessage;
                            panel.Children.Add( gridStack );
                            //panel.Children.Add( new Separator() );
                        }, DispatcherPriority.Render );
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler.ShowMessage( e );
            }
        }

        private void GridOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ( (MainWindow) Application.Current.MainWindow ).DisplayConcreteMail( (MailMessageProxy) ( ( sender as Grid ).Tag ) );
        }

        private StackPanel GetMailStack()
        {
            return new StackPanel { Focusable = false };
        }

        private TextBlock GetSenderBlock(string text)
        {
            return new TextBlock { Text = text, Style = senderStyle };
        }

        private TextBlock GetSubjectBlock(string text)
        {
            return new TextBlock { Text = text, Style = subjectStyle };
        }

        private TextBlock GetBodyBlock(string text)
        {
            return new TextBlock { Text = text, Style = bodyStyle };
        }

        private TextBlock GetDateBlock(DateTime receivedAt)
        {
            return new TextBlock { Text = receivedAt.ToString( "d" ), Style = dateStyle };
        }

        private TextBlock GetTimeBlock(DateTime receivedAt)
        {
            return new TextBlock { Text = receivedAt.ToString( "t" ), Style = dateStyle };
        }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            worker.Dispose();
        }

        #endregion

        #region Styles

        private readonly Style bodyStyle = Application.Current.Resources["MailBodyPreviewTextBlock"] as Style;
        private readonly Style senderStyle = Application.Current.Resources["MailSenderTextBlock"] as Style;
        private readonly Style stackStyle = Application.Current.Resources["MailStackPanel"] as Style;
        private readonly Style subjectStyle = Application.Current.Resources["MailSubjectTextBlock"] as Style;
        private readonly Style dateStyle = Application.Current.Resources["DateTextBlock"] as Style;

        #endregion
    }
}