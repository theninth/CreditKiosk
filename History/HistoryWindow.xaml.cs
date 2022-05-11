using CreditKiosk.Events;
using CreditKiosk.Models;
using Microsoft.EntityFrameworkCore;
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

namespace CreditKiosk.History
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public List<Person> Persons { get; set; }

        public HistoryWindow(List<Person> persons, Person selectedPerson)
        {
            this.Persons = persons;
            InitializeComponent();

            UpdateLbPersons();

            LbPersons.SelectedItem = LbPersons.Items
                .Cast<Person>()
                .ToList()
                .Where(i => i.Id == selectedPerson.Id)
                .Single();
        }

        public event EventHandler<CreditEventArgs>? PersonCredited;

        private void UpdateLbPersons()
        {
            LbPersons.Items.Clear();
            foreach (Person person in this.Persons)
            {
                LbPersons.Items.Add(person);
            }
        }

        private void UpdateLblBalance(Person? person)
        {
            if (person == null)
            {
                LblBalance.Content = string.Empty;
            }
            else
            {
                LblBalance.Content = $"Saldo: {person.Balance} kr";
            }
        }

        private void UpdateLvDepositsAndPurchases(Person person)
        {
            using (var context = new KioskDbContext())
            {
                if (context.Deposits == null || context.Purchases == null)
                {
                    throw new Exception("Neither Deposits nor Purchases should be null here. What happened?");
                }

                List<Deposit>? deposits = context.Deposits
                    .Where(p => p.PersonId == person.Id)
                    .ToList();

                List<Purchase>? purchases = context.Purchases
                    .Where(p => p.PersonId == person.Id)
                    .Include(d => d.Person)
                    .Include(d => d.ProductGroup)
                    .ToList();

                LvDeposits.Items.Clear();
                foreach (Deposit? deposit in deposits)
                {
                    if (deposit != null) LvDeposits.Items.Add(deposit);
                }

                LvPurchases.Items.Clear();
                foreach (Purchase? purchase in purchases)
                {
                    if (purchase != null) LvPurchases.Items.Add(purchase);
                }
            }
        }

        protected virtual void OnCredit(CreditEventArgs e) => PersonCredited?.Invoke(this, e);


        /* ************
        * GUI EVENTS *
        ************ */

        private void BtnCredit_Click(object sender, RoutedEventArgs e)
        {
            Purchase purchase = (Purchase)LvPurchases.SelectedItem;
            
            if (purchase == null) return;
            
            CreditPurchaseWindow frm = new(purchase);
            frm.ShowDialog();
            
            if (frm.DialogResult != null && (bool)frm.DialogResult)
            {
                CreditEventArgs creditEventArgs = new(frm.Purchase, frm.Amount);
                OnCredit(creditEventArgs);
                Close();
            }
            
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LbPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person? selectedPerson = (Person)LbPersons.SelectedItem;
            if (selectedPerson != null)
            {
                UpdateLvDepositsAndPurchases(selectedPerson);
            }

            UpdateLblBalance(selectedPerson);

        }

        private void LvDeposits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void LvPurchases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool purchaseSelected = LvPurchases.SelectedIndex >= 0;
            BtnCredit.IsEnabled = purchaseSelected;
        }
    }
}
