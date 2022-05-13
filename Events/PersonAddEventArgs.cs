using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Event arguments for when a person is added PersonsWindow.
    /// </summary>
    public class PersonAddEventArgs : PersonEventArgs
    {
        private double initialDeposit;

        /// <summary>
        /// The initial deposit amount.
        /// </summary>
        public double InitialDeposit {
            get
            {
                return initialDeposit;
            }
            set
            {
                if (value >= 0)
                {
                    initialDeposit = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Initial deposit can not be a negative number.");
                }
            }
        }

        /// <summary>
        /// Construcot
        /// </summary>
        /// <param name="person">Person object connected to the deposit.</param>
        /// <param name="initialDeposit">Amount to be deposit.</param>
        public PersonAddEventArgs(Person person, double initialDeposit) : base(person)
        {
            this.initialDeposit = initialDeposit;
        }
    }
}
