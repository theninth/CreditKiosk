using CreditKiosk.Models;
using System;

namespace CreditKiosk.Events
{
    /// <summary>
    /// Generiv Event arguments for something where a Person is needed to be supplied.
    /// </summary>
    public class PersonEventArgs : EventArgs
    {
        /// <summary>
        /// Supplied Person object.
        /// </summary>
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
