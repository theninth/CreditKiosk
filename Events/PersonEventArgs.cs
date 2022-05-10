using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    public class PersonEventArgs : EventArgs
    {
        public Person Person { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="person">Person object</param>
        public PersonEventArgs(Person person)
        {
            this.Person = person;
        }
    }
}
