using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sanet.Views
{
    public abstract class BasePage:Page//SwapChainBackgroundPanel
    {
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        public abstract void OnNavigatedTo();

        public abstract void OnNavigatedFrom();
        
        
        #region ViewModel
        public void SetViewModel<T>() where T : BaseViewModel
        {
            DataContext = ViewModelProvider.GetViewModel<T>();
            
        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            return (T)DataContext;
        }

        #endregion

        #region Visual state switching
        private List<Control> _layoutAwareControls;
        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded"/>
        /// event of a <see cref="Control"/> within the page, to indicate that the sender should
        /// start receiving visual state management changes that correspond to application view
        /// state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.  A corresponding
        /// <see cref="FrameworkElement.Unloaded"/> event handler connected to
        /// <see cref="StopLayoutUpdates"/> is strongly encouraged.  Instances of
        /// <see cref="LayoutAwarePage"/> automatically invoke these handlers in their Loaded and
        /// Unloaded events.</remarks>
        /// <seealso cref="DetermineVisualState"/>
        /// <seealso cref="InvalidateVisualState"/>
        public void StartLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._layoutAwareControls == null)
            {
                // Start listening to view state changes when there are controls interested in updates
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._layoutAwareControls = new List<Control>();
            }
            this._layoutAwareControls.Add(control);

            // Set the initial visual state of the control
            VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded"/>
        /// event of a <see cref="Control"/>, to indicate that the sender should start receiving
        /// visual state management changes that correspond to application view state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.</remarks>
        /// <seealso cref="StartLayoutUpdates"/>
        public void StopLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null || this._layoutAwareControls == null) return;
            this._layoutAwareControls.Remove(control);
            if (this._layoutAwareControls.Count == 0)
            {
                // Stop listening to view state changes when no controls are interested in updates
                this._layoutAwareControls = null;
                Window.Current.SizeChanged -= this.WindowSizeChanged;
            }
        }

        /// <summary>
        /// Translates <see cref="ApplicationViewState"/> values into strings for visual state
        /// management within the page.  The default implementation uses the names of enum values.
        /// Subclasses may override this method to control the mapping scheme used.
        /// </summary>
        /// <param name="viewState">View state for which a visual state is desired.</param>
        /// <returns>Visual state name used to drive the
        /// <see cref="VisualStateManager"/></returns>
        /// <seealso cref="InvalidateVisualState"/>
        protected virtual string DetermineVisualState(ApplicationViewState viewState)
        {
            return viewState.ToString();
        }

        /// <summary>
        /// Updates all controls that are listening for visual state changes with the correct
        /// visual state.
        /// </summary>
        /// <remarks>
        /// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
        /// signal that a different value may be returned even though the view state has not
        /// changed.
        /// </remarks>
        public void InvalidateVisualState()
        {
            if (this._layoutAwareControls != null)
            {
                string visualState = DetermineVisualState(ApplicationView.Value);
                foreach (var layoutAwareControl in this._layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
            //if (DataContext != null && DataContext is MenuViewModel)
            //    (DataContext as MenuViewModel).UpdateMenuWidth();
        }

        #endregion
    }
}
