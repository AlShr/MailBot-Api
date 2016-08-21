using System;
using MailBot.Client.Windows.Dialogues;

namespace MailBot.Client.ErrorHandling
{
    internal static class ErrorHandler
    {
        internal static void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            ShowMessage( (Exception) unhandledExceptionEventArgs.ExceptionObject );
        }

        internal static void ShowMessage(Exception e)
        {
            ShowMessage( "Error!", e.Message );
        }

        internal static void ShowMessage(string title, string message)
        {
            const string accentedText = "We're sorry about this ;'(";
            StatementWindow.ShowError( title, accentedText, message );
        }
    }
}