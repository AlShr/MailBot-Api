using System.Collections.Generic;
using System.Windows.Media;

namespace MailBot.Client.ControlHelpers
{
    public static class WindowExtensions
    {
        public static IEnumerable<Visual> GetChildren(this Visual parent, bool recurse = true)
        {
            if (parent != null)
            {
                var count = VisualTreeHelper.GetChildrenCount( parent );
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild( parent, i ) as Visual;
                    if (child != null)
                    {
                        yield return child;

                        if (recurse)
                        {
                            foreach (var grandChild in child.GetChildren())
                            {
                                yield return grandChild;
                            }
                        }
                    }
                }
            }
        }
    }
}