using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using MahApps.Metro.Controls;

namespace MailBot.Client.ControlHelpers
{
    public class UiDisabler : IDisposable
    {
        #region Instances list

        private static readonly List<UiDisabler> ActiveInstances = new List<UiDisabler>();

        #endregion

        #region Effects

        private static readonly BlurEffect DisabledBlurEffect = new BlurEffect { Radius = 2.5d, KernelType = KernelType.Gaussian, RenderingBias = RenderingBias.Performance };

        #endregion

        #region Functions

        private static readonly Action<FrameworkElement> EnablingAction = element =>
        {
            element.IsEnabled = true;
            element.Effect = null;
        };

        private static readonly Action<FrameworkElement> DisablingAction = element =>
        {
            element.IsEnabled = false;
            element.Effect = DisabledBlurEffect;
        };

        private static readonly Func<FrameworkElement, bool> DefaultFilteringRule = element => element != null && !( element is Grid ) && !( element is Panel ) && !( element is ProgressRing );

        #endregion

        #region Initialization

        private IEnumerable<FrameworkElement> controls;

        /// <exception cref="ArgumentException">You're trying to block same control in multiple instances of UiDisabler.</exception>
        public UiDisabler(IEnumerable<FrameworkElement> controls, Func<FrameworkElement, bool> filteringRule = null)
        {
            controls = controls.Where( filteringRule ?? DefaultFilteringRule );
            lock (ActiveInstances)
            {
                var registeredControls = ActiveInstances.SelectMany( instance => instance.controls );
                if (registeredControls.Intersect( controls ).Any())
                {
                    throw new ArgumentException( "You're trying to block same control in multiple instances of UiDisabler." );
                }
                this.controls = controls;
                ActiveInstances.Add( this );
            }
        }

        public void Dispose()
        {
            Enable();
            ActiveInstances.Remove( this );
            controls = null;
            GC.SuppressFinalize( this );
        }

        #endregion

        #region Enabling & Disabling

        public bool IsDisabling { get; private set; }

        public void Disable()
        {
            if (!IsDisabling)
            {
                IsDisabling = true;
                foreach (var control in controls)
                {
                    DisablingAction( control );
                }
            }
        }

        public void Enable()
        {
            if (IsDisabling)
            {
                foreach (var control in controls)
                {
                    EnablingAction( control );
                }
                IsDisabling = false;
            }
        }

        #endregion
    }
}