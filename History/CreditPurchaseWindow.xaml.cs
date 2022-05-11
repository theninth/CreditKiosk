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

namespace CreditKiosk.History
{
    /// <summary>
    /// Interaction logic for CreditPurchaseWindow.xaml
    /// </summary>
    public partial class CreditPurchaseWindow : Window
    {
        public Purchase Purchase { get; set; }

        public double Amount { get; private set; }

        public CreditPurchaseWindow(Purchase purchase)
        {
            this.Purchase = purchase;

            InitializeComponent();

            TbxAmount.Text = $"{Purchase.Amount:n2}";
            UpdatePurchaseLabels();
        }

        private void UpdatePurchaseLabels()
        {
            LblPerson.Content = Purchase.Person.ToString();
            LblPurchase.Content = Purchase.ToString();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnCredit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Doing more than one crediting per purchase makes it possible
            // to credit more than the initial purchase. This bug should be fixed.

            double amount;

            if (!Double.TryParse(TbxAmount.Text, out amount))
            {
                MessageBox.Show("Ej giltig siffra", "Fel!");
                return;
            }
            
            if (amount > Purchase.Amount)
            {
                MessageBox.Show("Det går inte att kreditera mer än inköpet motsvarar", "Fel!");
                return;
            }
            else if (amount < 0)
            {
                MessageBox.Show("Det går inte att kreditera negativa summor", "Fel!");
                return;
            }

            Amount = amount;
            DialogResult = true;
            Close();
        }
    }
}
