using CreditKiosk.Events;
using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CreditKiosk.Persons
{
    /// <summary>
    /// Interaction logic for PersonsWindow.xaml
    /// </summary>
    public partial class PersonsWindow : Window
    {
        public PersonsWindow(IEnumerable<Person> persons)
        {
            InitializeComponent();
            SetDataLbPersons(persons);

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        /// <summary>
        /// Subscribe to this event to watch when a new person is added.
        /// </summary>
        public event EventHandler<PersonAddEventArgs>? PersonAdded;

        /// <summary>
        /// Subscribe to this event to watch when a person is deleted.
        /// </summary>
        public event EventHandler<PersonEventArgs>? PersonDeleted;

        private void SetDataLbPersons(IEnumerable<Person> persons)
        {
            LbPersons.Items.Clear();
            foreach (Person person in persons)
            {
                LbPersons.Items.Add(person);
            }
        }

        /// <summary>
        /// Fire events.
        /// </summary>
        /// <param name="e">Person add event arguments.</param>
        protected virtual void OnPersonAdded(PersonAddEventArgs e) => PersonAdded?.Invoke(this, e);

        /// <summary>
        /// Fire events.
        /// </summary>
        /// <param name="e">Generic Person event arguments.</param>
        protected virtual void OnPersonDeleted(PersonEventArgs e) => PersonDeleted?.Invoke(this, e);

        /// <summary>
        /// Event handler for Button add click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow frm = new();
            frm.ShowDialog();

            if (frm.Person != null)
            {
                PersonAddEventArgs personAddEventArgs = new(frm.Person, frm.InitialDeposit);
                OnPersonAdded(personAddEventArgs);
                LbPersons.Items.Add(frm.Person);
            }
        }

        /// <summary>
        /// Event handler for Button delete click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int noOfTransactions = 0;
            MessageBoxImage icon;
            string message;
            string title = "Är du säker?";
            Person? person = LbPersons.SelectedItem as Person;

            if (person == null) return;

            using (var context = new KioskDbContext())
            {
                noOfTransactions += context.Deposits.Where(i => i.PersonId == person.Id).Count();
                noOfTransactions += context.Purchases.Where(i => i.PersonId == person.Id).Count();
            }

            if (noOfTransactions > 0)
            {
                message = "Kunden har köpt kopplade till sig. Om du tar bort denne försvinner dessa." +
                    Environment.NewLine + Environment.NewLine +
                    "Är du helt säker på att du vill radera kunden?";
                icon = MessageBoxImage.Warning;
            } 
            else
            {
                message = "Är du säker på att du vill radera person (inga köp påverkas)?";
                icon = MessageBoxImage.Information;
            }

            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, icon);
            if (result == MessageBoxResult.Yes)
            {
                PersonEventArgs personEventArgs = new PersonEventArgs(person);
                OnPersonDeleted(personEventArgs);
                LbPersons.Items.Remove(person);
            }
            
        }

        /// <summary>
        /// Event handler for Button done click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BtnDone_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
