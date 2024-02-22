using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for SpinButton.xaml
    /// </summary>
    /// 

    public partial class SpinButton : Canvas
    {



        private const decimal Max = 2;
        private const decimal Min = -2;

        private bool loaded;

        private bool leftButtonUp;
        private bool taskRunning;

        public SpinButton()
        {
            InitializeComponent();
        }

        private void Increase()
        {
            decimal newVal = this.Value + Increment;
            if (newVal <= MaxValue)
                this.Value += this.Increment;
        }

        private void Decrease()
        {
            decimal newVal = this.Value - Increment;
            if (newVal >= MinValue)
                this.Value -= this.Increment;
        }

        private void increment_Click(object sender, RoutedEventArgs e)
        {
            Increase();
        }

        private void decrement_Click(object sender, RoutedEventArgs e)
        {
            Decrease();
        }

        #region properties 

        public RelayCommand Command
        {
            get { return (RelayCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(RelayCommand), typeof(SpinButton), new PropertyMetadata(default(RelayCommand)));
        //public string ValueText
        //{
        //    get { return (string)GetValue(ValueTextProperty); }
        //    set { SetValue(ValueTextProperty, value); }
        //}

        //public static readonly DependencyProperty ValueTextProperty =
        //    DependencyProperty.Register("ValueText", typeof(string), typeof(SpinButton), new PropertyMetadata(default(string)));


        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(
        "Value", typeof(decimal), typeof(SpinButton),
        new FrameworkPropertyMetadata(Min, new PropertyChangedCallback(OnValueChanged),

            new CoerceValueCallback(CoerceValue)));

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
    DependencyProperty.Register("Enabled", typeof(bool), typeof(SpinButton), new PropertyMetadata(default(bool)));

        public decimal Increment
        {
            get { return (decimal)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(decimal), typeof(SpinButton), new PropertyMetadata(default(decimal)));


        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(decimal), typeof(SpinButton), new PropertyMetadata(default(decimal)));


        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(decimal), typeof(SpinButton), new PropertyMetadata(default(decimal)));
        #endregion


        private static object CoerceValue(DependencyObject element, object value)
        {
            decimal newValue = (decimal)value;
            return newValue;
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SpinButton control = (SpinButton)obj;

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                (decimal)args.OldValue, (decimal)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(SpinButton));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            if (loaded)
            {
                RaiseEvent(args);
            }
            else
            {
                loaded = true;
            }
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Increase();
            }
            else
            {
                Decrease();
            }
        }
        private void decrement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            RaiseDelayedAction(() => this.Value = this.MinValue, 500);

        }

        private void decrement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftButtonUp = true;
        }

        private void increment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            RaiseDelayedAction(() => this.Value = this.MaxValue, 500);
   
        }


        private void increment_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftButtonUp = true;
        }


        private void RaiseDelayedAction(Action action, long delay) //this gives super precise playback timings for chords!
        {
            if (taskRunning)
                return;

            taskRunning = true;

            Task.Run(() =>
           {
               leftButtonUp = false;

               var sw = new Stopwatch();

               sw.Reset();
               sw.Start();

               while (true)
               {
                   if (sw.ElapsedMilliseconds >= delay || leftButtonUp)
                   {
                       sw.Stop();
                       break;
                   }
               }

               if (!leftButtonUp)
                   Dispatcher.Invoke(() =>
                    {
                        action();
                    });

               taskRunning = false;
           });
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Value = 0;
        }
    }
}
