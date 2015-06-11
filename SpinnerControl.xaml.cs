using System;
using System.Windows;
using System.Windows.Controls;

namespace PhantomUI
{
    /// <summary>
    /// Interaction logic for SpinnerControl.xaml
    /// </summary>
    public partial class SpinnerControl : UserControl
    {
        #region Custom Events
        // https://msdn.microsoft.com/en-us/library/ms752288(v=vs.110).aspx
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy 
        public static readonly RoutedEvent StartAnimationEvent = EventManager.RegisterRoutedEvent("StartAnimation", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SpinnerControl));
        public static readonly RoutedEvent StopAnimationEvent  = EventManager.RegisterRoutedEvent("StopAnimation" , RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SpinnerControl));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler StartAnimation
        {
            add    { AddHandler(StartAnimationEvent   , value); }
            remove { RemoveHandler(StartAnimationEvent, value); }
        }

        public event RoutedEventHandler StopAnimation
        {
            add    { AddHandler(StopAnimationEvent   , value); }
            remove { RemoveHandler(StopAnimationEvent, value); }
        }

        // This method raises the Tap event 
        public void RaiseStartAnimationEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SpinnerControl.StartAnimationEvent);
            RaiseEvent(newEventArgs);
        }

        public void RaiseStopAnimationEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SpinnerControl.StopAnimationEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion

        public SpinnerControl()
        {
            InitializeComponent();
        }
    }
}