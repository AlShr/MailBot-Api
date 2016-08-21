using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace MailBot.Client.ControlHelpers
{
    public static class ControlValidator
    {
        private static readonly Brush InvalidBrush = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) );
        private static readonly Brush ValidBrush = new SolidColorBrush( Colors.White );

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="shouldntThrow" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="responsibleControl" /> is <see langword="null" />.</exception>
        public static void AssertAndBlame(Action shouldntThrow, Control responsibleControl, ref bool isValid)
        {
            if (shouldntThrow == null)
            {
                throw new ArgumentNullException( "shouldntThrow" );
            }
            if (responsibleControl == null)
            {
                throw new ArgumentNullException( "responsibleControl" );
            }
            try
            {
                shouldntThrow();
                PaintGood( responsibleControl );
            }
            catch (ArgumentException e)
            {
                PaintBad( responsibleControl, e.Message );
                isValid = false;
            }
        }

        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        /// ///
        /// <exception cref="ArgumentNullException"><paramref name="hasToBeTrue" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="responsibleControl" /> is <see langword="null" />.</exception>
        public static void AssertAndBlame(Func<bool> hasToBeTrue, string failMessage, Control responsibleControl, ref bool isValid)
        {
            if (hasToBeTrue == null)
            {
                throw new ArgumentNullException( "hasToBeTrue" );
            }
            if (responsibleControl == null)
            {
                throw new ArgumentNullException( "responsibleControl" );
            }
            try
            {
                if (!hasToBeTrue())
                {
                    PaintGood( responsibleControl );
                }
                else
                {
                    PaintBad( responsibleControl, failMessage );
                    isValid = false;
                }
            }
            catch (ArgumentException e)
            {
                PaintBad( responsibleControl, e.Message );
                isValid = false;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="control" /> is <see langword="null" />.</exception>
        public static void PaintGood(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException( "control" );
            }
            if (control.ToolTip != null)
            {
                control.Background = ValidBrush;
                control.ToolTip = null;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="control" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        public static void PaintBad(Control control, string message)
        {
            if (control == null)
            {
                throw new ArgumentNullException( "control" );
            }
            if (message == null)
            {
                throw new ArgumentNullException( "message" );
            }
            control.Background = InvalidBrush;
            control.ToolTip = message;
        }
    }
}