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
        /// <summary>
        /// Selected Person object.
        /// </summary>
        public List<Person> Persons { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="persons">List of Person objects.</param>
        /// <param name="selectedPerson">Initial selected person.</param>
        public HistoryWindow(List<Person> persons, Person selectedPerson)
        {
            this.Persons = persons;
            InitializeComponent();

            UpdateLbPersons();

            // Sets initialy selected person.
            LbPersons.SelectedItem = LbPersons.Items
                .Cast<Person>()
                .ToList()
                .Where(i => i.Id == selectedPerson.Id)
                .Single();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        /// <summary>
        /// Subscribe to get info when person is credited.
        /// </summary>
        public event EventHandler<CreditEventArgs>? PersonCredited;

        public event EventHandler<PersonEventArgs>? PersonSelectionChanged;

        private void UpdateLbPersons()
        {
            LbPersons.Items.Clear();
            foreach (Person person in this.Persons)
            {
                LbPersons.Items.Add(person);
            }
        }

        /// <summary>
        /// Set Balance label for a certain person object.
        /// </summary>
        /// <param name="person">Person to set label for.</param>
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

        /// <summary>
        /// Update lists to contain data for the Person object selected.
        /// </summary>
        /// <param name="person">Person object to set lists for.</param>
        /// <exception cref="Exception">Some tables seems to be empty. This should not happen.</exception>
        private void UpdateLists(Person person)
        {
            using (var context = new KioskDbContext())
            {
                if (context.Deposits == null)
                {
                    throw new Exception("Deposits should not be null here. What happened?");
                }
                else if (context.Purchases == null)
                {
                    throw new Exception("Purchases should not be null here. What happened?");
                }
                else if (context.Credits == null)
                {
                    throw new Exception("Credits should not be null here. What happened?");
                }

                List<Deposit>? deposits = context.Deposits
                    .Where(p => p.PersonId == person.Id)
                    .ToList();

                List<Purchase>? purchases = context.Purchases
                    .Where(p => p.PersonId == person.Id)
                    .Include(d => d.Person)
                    .Include(d => d.ProductGroup)
                    .ToList();

                List<Credit>? credits = context.Credits
                    .Where(c => c.Purchase.PersonId == person.Id)
                    .Include(c => c.Purchase)
                    .Include(c => c.Purchase.Person)
                    .Include(c => c.Purchase.ProductGroup)
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

                LvCredits.Items.Clear();
                foreach (Credit? credit in credits)
                {
                    if (credit != null) LvCredits.Items.Add(credit);
                }
            }
        }

        /// <summary>
        /// Fire events for when a person is credited.
        /// </summary>
        /// <param name="e">Credit event arguments.</param>
        protected virtual void OnCredit(CreditEventArgs e) => PersonCredited?.Invoke(this, e);

        /// <summary>
        /// Fire events for when a new person is selected.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPersonSelectionChanged(PersonEventArgs e) => PersonSelectionChanged?.Invoke(this, e);

        /*************
        * GUI EVENTS *
        ************ */

        /// <summary>
        /// Fires when credit button was clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnCredit_Click(object sender, RoutedEventArgs e)
        {
            Purchase purchase = (Purchase)LvPurchases.SelectedItem;
            
            if (purchase == null) return;

            double? creditableAmount = purchase.CreditableAmount;
            if (creditableAmount == null)
            {
                throw new Exception("Creditable amount was null. This should not be able to happen.");
            }

            if (creditableAmount <= 0)
            {
                MessageBox.Show("Kan inte kreditera. Varan är redan helt krediterad!", "Fel!");
                return;
            }

            CreditPurchaseWindow frm = new(purchase);
            frm.ShowDialog();
            
            if (frm.DialogResult != null && (bool)frm.DialogResult)
            {
                CreditEventArgs creditEventArgs = new(frm.Purchase, frm.Amount);
                OnCredit(creditEventArgs);
                Close();
            }
            
        }

        /// <summary>
        /// Fires when done button was clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Fires when a diffrent person was selected.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LbPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person? selectedPerson = (Person)LbPersons.SelectedItem;
            if (selectedPerson != null)
            {
                OnPersonSelectionChanged(new PersonEventArgs(selectedPerson));
                UpdateLists(selectedPerson);
                UpdateLblBalance(selectedPerson);
            }
        }

        /// <summary>
        /// Fires when a diffrent deposit was selected.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LvDeposits_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        /// <summary>
        /// Fires when a different purchase was selected.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LvPurchases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool purchaseSelected = LvPurchases.SelectedIndex >= 0;
            BtnCredit.IsEnabled = purchaseSelected;
        }

        /// <summary>
        /// Fires when a different tab is chosen (and when the tab control childrens are
        /// changing selection, but those are ignored by the method).
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Really ugly hack to make sure the changes are in the tabcontrol and not the children.
            if (!((Control)e.Source).GetType().ToString().EndsWith("TabControl")) return;

            TabItem currentTabItem = (TabItem)((TabControl)e.Source).SelectedItem;
            if (currentTabItem == null) return;  // Should not happen

            // Make check if button should be shown or not.
            BtnCredit.Visibility = currentTabItem.Name == "TabItemPurchases" ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
