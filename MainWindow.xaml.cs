using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CreditKiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Managers.PersonManager personManager = new();
        Managers.ProductGroupManager productGroupManager = new();
        Managers.TransactionManager transactionManager = new();

        public MainWindow()
        {
            InitializeComponent();
            UpdateListBoxPerson();

            //using (var context = new KioskDbContext())
            //{
            //    var person = context.Persons.First();
            //    var deposit = new Deposit()
            //    {
            //        PersonId = person.Id,
            //        Amount = -5,
            //    };
            //    transactionManager.Deposit(deposit);
            //    Debug.WriteLine($"Saldo: {personManager.GetBalance(person)}");
            //}
        }

        private void OpenPurchase()
        {
            // This should not be able to happen because button should not be enabled.
            if (ListboxPersons.SelectedItem == null) return;

            PurchaseWindow.PurchaseWindow purchaseWindow = new()
            {
                ProductGroups = productGroupManager.GetList().ToArray(),
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

        private void UpdateListBoxPerson()
        {
            ListboxPersons.Items.Clear();
            foreach (Person person in personManager.GetAllSorted())
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

        private void ListboxPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLabelBalance();
        }

        private void StartPurchase_Click(object sender, RoutedEventArgs e) => OpenPurchase();

        private void ListboxPersons_MouseDoubleClick(object sender, MouseButtonEventArgs e) => OpenPurchase();
    }
}
