using CreditKiosk.Enums;
using System;

namespace CreditKiosk.Events
{
    public class NumPadPressedEventArgs : EventArgs
    {
        public NumPadButtons NumPadButton { get; set; }
        public int Digit { get; set; } = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="button">Numpad button pressed</param>
        public NumPadPressedEventArgs(NumPadButtons button)
        {
            if (button == NumPadButtons.Digit)
            {
                throw new ArgumentException("When numpad button is Digit, a digit must be supplied to the constructor.");
            }
            this.NumPadButton = button;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="button">Numpad button pressed</param>
        public NumPadPressedEventArgs(NumPadButtons button, int digit)
        {
            this.NumPadButton = button;
            this.Digit = digit;
        }
    }
   
}
