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
        private double amountAvailableForCredit;

        public Purchase Purchase { get; set; }

        public double Amount { get; private set; }

        public CreditPurchaseWindow(Purchase purchase)
        {
            this.Purchase = purchase;
            InitializeComponent();

            TbxAmount.Text = $"{Purchase.CreditableAmount:n2}";
            UpdateLabels();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        private void UpdateLabels()
        {
            LblPerson.Content = Purchase.Person != null ? Purchase.Person.ToString() : string.Empty;
            LblPurchase.Content = Purchase.ToString();
            LblAvailableForCredit.Content = $"{Purchase.CreditableAmount:n2}";
        }

        private void UpdateControlsOnValid()
        {
            double amount;

            bool isValidText = Double.TryParse(TbxAmount.Text, out amount);
            bool isValidAmount = amount <= Purchase.CreditableAmount;

            // Sets background color of textbox.
            TbxAmount.Background = isValidText && isValidAmount ?
                new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);

            // Enables or disables credit button.
            BtnCredit.IsEnabled = isValidText && isValidAmount;
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
            
            if (amount > Purchase.CreditableAmount)
            {
                MessageBox.Show($"Det går inte att kreditera mer än {Purchase.CreditableAmount:n2} kr", "Fel!");
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

        private void TbxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateControlsOnValid();
        }
    }
}
