using System.Windows;

namespace MailBot.Client.ControlHelpers
{
    public static class ControlExtensions
    {
        public static Window SetLocation(this Window window, double left, double top)
        {
            window.Left = left;
            window.Top = top;
            return window;
        }
    }
}