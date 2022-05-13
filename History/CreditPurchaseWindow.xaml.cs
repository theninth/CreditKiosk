using CreditKiosk.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CreditKiosk.History
{
    /// <summary>
    /// Interaction logic for CreditPurchaseWindow.xaml
    /// </summary>
    public partial class CreditPurchaseWindow : Window
    {
        /// <summary>
        /// The purchase that should be credited.
        /// </summary>
        public Purchase Purchase { get; set; }

        /// <summary>
        /// Amount to be credited.
        /// </summary>
        public double Amount { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="purchase">The purchase that should be credited.</param>
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

        /// <summary>
        /// Makes labels show current values.
        /// </summary>
        private void UpdateLabels()
        {
            LblPerson.Content = Purchase.Person != null ? Purchase.Person.ToString() : string.Empty;
            LblPurchase.Content = Purchase.ToString();
            LblAvailableForCredit.Content = $"{Purchase.CreditableAmount:n2}";
        }

        /// <summary>
        /// Makes control behave diffrently depending on if amount is valid or not.
        /// </summary>
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

        /// <summary>
        /// Event handler for Cancel button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Event handler for Credit button click.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args</param>
        private void BtnCredit_Click(object sender, RoutedEventArgs e)
        {
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

        /// <summary>
        /// Event handler for when text in amount text box is changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event args</param>
        private void TbxAmount_TextChanged(object sender, TextChangedEventArgs e) => UpdateControlsOnValid();
    }
}
