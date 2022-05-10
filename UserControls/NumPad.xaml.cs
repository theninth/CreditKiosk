using CreditKiosk.Events;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CreditKiosk.UserControls
{
    /// <summary>
    /// Interaction logic for NumPad.xaml
    /// </summary>
    public partial class NumPad : UserControl
    {
        public NumPad()
        {
            InitializeComponent();

            // Makes sure decimal separator is locale independent.
            BtnComma.Content = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }

        /// <summary>
        /// Subscribe to this event to watch when numpad is pressed
        /// </summary>
        public event EventHandler<NumPadPressedEventArgs>? NumPadPressed;

        /// <summary>
        /// When button is pressed
        /// </summary>
        /// <param name="e">Event args for NumPad buttons pressed</param>
        protected virtual void OnBtn(NumPadPressedEventArgs e)
        {
            if (NumPadPressed != null) NumPadPressed(this, e);
        }

        private void Btn0_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 0));

        private void Btn1_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 1));


        private void Btn2_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 2));

        private void Btn3_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 3));

        private void Btn4_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 4));

        private void Btn5_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 5));

        private void Btn6_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 6));

        private void Btn7_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 7));

        private void Btn8_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 8));

        private void Btn9_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 9));

        private void BtnComma_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.DecimalSeparator));

        private void BtnClear_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Clear));
    }
}
