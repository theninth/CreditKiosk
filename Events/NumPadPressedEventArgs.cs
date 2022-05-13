using CreditKiosk.Enums;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Event arguments for a button press on a num pad.
    /// </summary>
    public class NumPadPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Kind of num pad button pressed (example: Digit, Clear or Comma).
        /// </summary>
        public NumPadButtons NumPadButton { get; set; }
        
        /// <summary>
        /// If button pressed is a digit, it will be supplied with this property.
        /// </summary>
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
