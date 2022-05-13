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

        /// <summary>
        /// Event handler for when Button "0" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn0_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 0));

        /// <summary>
        /// Event handler for when Button "1" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn1_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 1));

        /// <summary>
        /// Event handler for when Button "2" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn2_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 2));

        /// <summary>
        /// Event handler for when Button "3" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn3_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 3));

        /// <summary>
        /// Event handler for when Button "4" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn4_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 4));

        /// <summary>
        /// Event handler for when Button "5" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn5_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 5));

        /// <summary>
        /// Event handler for when Button "6" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn6_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 6));

        /// <summary>
        /// Event handler for when Button "7" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn7_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 7));

        /// <summary>
        /// Event handler for when Button "8" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn8_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 8));

        /// <summary>
        /// Event handler for when Button "9" is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void Btn9_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Digit, 9));

        /// <summary>
        /// Event handler for when Button for decimal separator is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void BtnComma_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.DecimalSeparator));

        /// <summary>
        /// Event handler for when Button for clear is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        private void BtnClear_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Clear));
    }
}
