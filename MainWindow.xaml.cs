using CreditKiosk.Events;
using CreditKiosk.History;
using CreditKiosk.Models;
using CreditKiosk.Persons;
using CreditKiosk.ProductGroups;
using System.Collections.Generic;
using System.Diagnostics;
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
        Managers.PersonManager personManager;
        Managers.ProductGroupManager productGroupManager;
        Managers.TransactionManager transactionManager;

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
        }

        private void OpenPurchase()
        {
            // This should not be able to happen because button should not be enabled.
            if (ListboxPersons.SelectedItem == null) return;

            PurchaseWindow.PurchaseWindow purchaseWindow = new()
            {
                ProductGroups = productGroupManager.GetAll().ToArray(),
                Person = (Person)ListboxPersons.SelectedItem
            };
            purchaseWindow.ShowDialog();

            if (purchaseWindow.Purchases == null)
            {
                Debug.WriteLine("Purchase window ended without supplied purchase. I suppose user aborted?");
                return;
            }

            foreach (Purchase? purchase in purchaseWindow.Purchases)
            {
                transactionManager.Purchase(purchase);
            }

            UpdateLabelBalance();
        }

        private void UpdateBtnVisibility()
        {
            bool itemIsSelected = ListboxPersons.SelectedIndex >= 0;
            BtnStartPurchase.IsEnabled = itemIsSelected;
            BtnHistory.IsEnabled = itemIsSelected;
        }

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

        private void UpdateLabelBalance()
        {
            if (ListboxPersons.SelectedItem == null)
            {
                LabelBalance.Content = "";
                return;
            }

            double balance = ((Person)ListboxPersons.SelectedItem).Balance;
            LabelBalance.Content = $"Saldo: {balance} Kr.";
        }


        /* ***************
         * CUSTOM EVENTS *
         *************** */

        private void OnProductGroupAdded(object source, ProductGroupEventArgs e)
        {
            if (e.ProductGroup != null) productGroupManager.Add(e.ProductGroup);
        }

        private void OnProductGroupDeleted(object source, ProductGroupEventArgs e)
        {
            if (e.ProductGroup != null) productGroupManager.Remove(e.ProductGroup);
        }

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

        private void OnPersonDeleted(object source, PersonEventArgs e)
        {
            if (e.Person != null) personManager.Remove((Person)e.Person);
            UpdateListBoxPerson();
        }

        /* ************
         * GUI EVENTS *
         ************ */


        private void BtnProductGroups_Click(object sender, RoutedEventArgs e)
        {
            ProductGroupsWindow frm = new(productGroupManager.GetAll());
            frm.ProductGroupAdded += OnProductGroupAdded;
            frm.ProductGroupDeleted += OnProductGroupDeleted;
            frm.ShowDialog();
        }

        private void BtnPersons_Click(object sender, RoutedEventArgs e)
        {
            PersonsWindow frm = new PersonsWindow(personManager.GetAll());
            frm.PersonAdded += OnPersonAdded;
            frm.PersonDeleted += OnPersonDeleted;
            frm.ShowDialog();
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListboxPersons.SelectedIndex;

            HistoryWindow frm = new(personManager.GetAll(), selectedIndex);
            frm.ShowDialog();
        }

        private void BtnStartPurchase_Click(object sender, RoutedEventArgs e) => OpenPurchase();

        private void ListboxPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBtnVisibility();
            UpdateLabelBalance();
        }

        private void ListboxPersons_MouseDoubleClick(object sender, MouseButtonEventArgs e) => OpenPurchase();
    }
}
