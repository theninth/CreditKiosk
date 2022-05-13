using CreditKiosk.Events;
using CreditKiosk.History;
using CreditKiosk.Models;
using CreditKiosk.Persons;
using CreditKiosk.ProductGroups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CreditKiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Managers.PersonManager personManager;
        private Managers.ProductGroupManager productGroupManager;
        private Managers.TransactionManager transactionManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            using (var context = new KioskDbContext())
            {
                context.Database.EnsureCreated();
            }

            productGroupManager = new();
            personManager = new();
            transactionManager = new();
            UpdateListBoxPerson();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        /// <summary>
        /// Run when a new purchase is started.
        /// </summary>
        private void OpenPurchase()
        {
            // This should not be able to happen because button should not be enabled.
            if (ListboxPersons.SelectedItem == null) return;

            PurchaseWindow.PurchaseWindow purchaseWindow = new()
            {
                ProductGroups = productGroupManager.GetAll().ToArray(),
                Person = (Person)ListboxPersons.SelectedItem
            };

            purchaseWindow.NewPurchase += OnNewPurchase;
            purchaseWindow.ShowDialog();
        }

        /// <summary>
        /// Set if buttons are enabled or disabled depending on if any items are selected.
        /// </summary>
        private void UpdateBtnEnabledDisabled()
        {
            bool itemIsSelected = ListboxPersons.SelectedIndex >= 0;
            BtnDeposit.IsEnabled = itemIsSelected;
            BtnHistory.IsEnabled = itemIsSelected;
            BtnStartPurchase.IsEnabled = itemIsSelected;
        }

        /// <summary>
        /// Updates the content in listbox ListboxPersons from content in personManager.
        /// </summary>
        private void UpdateListBoxPerson()
        {
            ListboxPersons.Items.Clear();
            List<Person> persons = personManager.GetAllSorted();

            if (persons == null) return;

            foreach (Person person in persons)
            {
                ListboxPersons.Items.Add(person);
            }
        }

        /// <summary>
        /// Updates balance label content.
        /// </summary>
        private void UpdateLabelBalance()
        {
            if (ListboxPersons.SelectedItem == null)
            {
                LabelBalance.Content = "";
                return;
            }

            double balance = ((Person)ListboxPersons.SelectedItem).Balance;
            LabelBalance.Content = $"Saldo: {balance:n2} Kr.";
        }


        /*****************
         * CUSTOM EVENTS *
         *************** */

        /// <summary>
        /// Event handler for when a ProductGroup is added.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnProductGroupAdded(object source, ProductGroupEventArgs e)
        {
            if (e.ProductGroup != null) productGroupManager.Add(e.ProductGroup);
        }

        /// <summary>
        /// Event handler for when a ProductGroup is deleted.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnProductGroupDeleted(object source, ProductGroupEventArgs e)
        {
            if (e.ProductGroup != null) productGroupManager.Remove(e.ProductGroup);
        }

        /// <summary>
        /// Event handler for when a Person is added.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnPersonAdded(object source, PersonAddEventArgs e)
        {
            if (e.Person != null) personManager.Add((Person)e.Person);
            if (e.Person != null && e.InitialDeposit > 0)
            {
                Deposit deposit = new()
                {
                    PersonId = e.Person.Id,
                    Amount = e.InitialDeposit,
                    Comment = "Startkredit"
                };
                transactionManager.Deposit(deposit);
            }

            UpdateListBoxPerson();
        }

        /// <summary>
        /// Event handler for when a Person is deleted.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnPersonDeleted(object source, PersonEventArgs e)
        {
            if (e.Person != null) personManager.Remove((Person)e.Person);
            UpdateListBoxPerson();
        }

        /// <summary>
        /// Event handler for when another window changes what Person is selected.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnPersonSelectionChanged(object source, PersonEventArgs e)
        {
            // Selects the person in ListBox with the same Id (i. e. the same person).
            ListboxPersons.SelectedItem = ListboxPersons.Items.Cast<Person>().ToList().Where(i => i.Id == e.Person.Id).Single();
        }

        /// <summary>
        /// Event handler for when a transaction is credited.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnCredited(object source, CreditEventArgs e)
        {
            if (e.Purchase == null) return;

            try
            {
                transactionManager.Credit(e.Purchase, e.Amount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fel!");
            }
            UpdateLabelBalance();
        }

        /// <summary>
        /// Event handler for a new purchase.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="e">Event handler.</param>
        private void OnNewPurchase(object source, PurchaseEventArgs e)
        {
            transactionManager.Purchase(e.Purchase);
            UpdateLabelBalance();
        }

        /**************
         * GUI EVENTS *
         **************/

        /// <summary>
        /// Event handler for exit button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult frm = MessageBox.Show(
                "Vill du verkligen avsluta?", "Avsluta", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (frm == MessageBoxResult.Yes) Close();
        }

        /// <summary>
        /// Event handler for Product group button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnProductGroups_Click(object sender, RoutedEventArgs e)
        {
            ProductGroupsWindow frm = new(productGroupManager.GetAll());
            frm.ProductGroupAdded += OnProductGroupAdded;
            frm.ProductGroupDeleted += OnProductGroupDeleted;
            frm.ShowDialog();
        }

        /// <summary>
        /// Event handler for persons button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnPersons_Click(object sender, RoutedEventArgs e)
        {
            PersonsWindow frm = new PersonsWindow(personManager.GetAllSorted());
            frm.PersonAdded += OnPersonAdded;
            frm.PersonDeleted += OnPersonDeleted;
            frm.ShowDialog();
        }

        /// <summary>
        /// Event handler for history button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            Person selectedPerson = (Person)ListboxPersons.SelectedItem;

            HistoryWindow frm = new(personManager.GetAllSorted(), selectedPerson);
            frm.PersonCredited += OnCredited;
            frm.PersonSelectionChanged += OnPersonSelectionChanged;
            frm.ShowDialog();
        }

        /// <summary>
        /// Event handler for start purchase button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnStartPurchase_Click(object sender, RoutedEventArgs e) => OpenPurchase();

        /// <summary>
        /// Event handler for when selection changed in the listbox ListboxPersons
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void ListboxPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBtnEnabledDisabled();
            UpdateLabelBalance();
        }

        /// <summary>
        /// Event handler for when item in listbox ListboxPersons i double clicked (i. e. start new pay).
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void ListboxPersons_MouseDoubleClick(object sender, MouseButtonEventArgs e) => OpenPurchase();

        /// <summary>
        /// Event handler for button deposit is clicked.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event handler.</param>
        private void BtnDeposit_Click(object sender, RoutedEventArgs e)
        {
            Person selectedPerson = (Person)ListboxPersons.SelectedItem;

            if (selectedPerson == null) return;

            DepositWindow.DepositWindow frm = new(selectedPerson);
            frm.ShowDialog();

            if (frm.DialogResult != null && (bool)frm.DialogResult && frm.Deposit != null)
            {
                transactionManager.Deposit(frm.Deposit);
            }
            UpdateLabelBalance();
        }
    }
}
