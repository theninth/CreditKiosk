using CreditKiosk.Enums;
using System;

namespace CreditKiosk.Events
{
    public class NumPadPressedEventArgs : EventArgs
    {
        public NumPadButtons NumPadNutton { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="button">Numpad button pressed</param>
        public NumPadPressedEventArgs(NumPadButtons button)
        {
            this.NumPadNutton = button;
        }
    }
   
}
