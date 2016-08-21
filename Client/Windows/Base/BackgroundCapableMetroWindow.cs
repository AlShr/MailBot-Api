using System;
using System.ComponentModel;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MailBot.Client.ControlHelpers;

namespace MailBot.Client.Windows.Base
{
    public abstract class BackgroundCapableMetroWindow : MetroWindow
    {
        protected BackgroundCapableMetroWindow()
        {
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
        }

        protected UiDisabler UiDisabler { get; set; }
        protected BackgroundWorker Worker { get; private set; }

        protected void InvokeDispatcher(Action action)
        {
            App.Current.Dispatcher.InvokeAsync(action, DispatcherPriority.Background);
        }
    }
}