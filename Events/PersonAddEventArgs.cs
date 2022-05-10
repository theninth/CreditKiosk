using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    public class PersonAddEventArgs : PersonEventArgs
    {
        private double initialDeposit;

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

        public PersonAddEventArgs(Person person, double initialDeposit) : base(person)
        {
            this.initialDeposit = initialDeposit;
        }
    }
}
