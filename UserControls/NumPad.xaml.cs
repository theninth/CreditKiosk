using CreditKiosk.Events;
using System;
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
        }

        /// <summary>
        /// Subscribe to this event to watch when numpad is pressed
        /// </summary>
        public event EventHandler<NumPadPressedEventArgs> NumPadPressed;

        /// <summary>
        /// When button is pressed
        /// </summary>
        /// <param name="e">Event args for NumPad buttons pressed</param>
        protected virtual void OnBtn(NumPadPressedEventArgs e)
        {
            if (NumPadPressed != null) NumPadPressed(this, e);
        }

        private void Btn0_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Zero));

        private void Btn1_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.One));


        private void Btn2_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Two));

        private void Btn3_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Three));

        private void Btn4_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Four));

        private void Btn5_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Five));

        private void Btn6_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Six));

        private void Btn7_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Seven));

        private void Btn8_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Eight));

        private void Btn9_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Nine));

        private void BtnComma_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Comma));

        private void BtnClear_Click(object sender, RoutedEventArgs e) => OnBtn(new NumPadPressedEventArgs(Enums.NumPadButtons.Clear));
    }
}
