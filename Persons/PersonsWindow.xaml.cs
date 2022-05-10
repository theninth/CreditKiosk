﻿using CreditKiosk.Events;
using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        protected virtual void OnPersonAdded(PersonAddEventArgs e) => PersonAdded?.Invoke(this, e);

        protected virtual void OnPersonDeleted(PersonEventArgs e) => PersonDeleted?.Invoke(this, e);

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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Person? person = LbPersons.SelectedItem as Person;

            if (person == null) return;
            
            PersonEventArgs personEventArgs = new PersonEventArgs(person);
            OnPersonDeleted(personEventArgs);
            LbPersons.Items.Remove(person);
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
