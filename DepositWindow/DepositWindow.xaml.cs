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

namespace CreditKiosk.DepositWindow
{
    /// <summary>
    /// Interaction logic for DepositWindow.xaml
    /// </summary>
    public partial class DepositWindow : Window
    {
        private Person Person { get; set; }

        public Deposit? Deposit { get; set; }

        public DepositWindow(Person person)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            InitializeComponent();
            UpdateLabelPerson();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        private void UpdateControlsOnValid()
        {
            double amount;

            bool isValidText = Double.TryParse(TbxAmount.Text.Trim(), out amount);
            bool isSaneAmount = amount <= (double)App.Current.Resources["SaneAmount"];
            bool isAboveZero = amount > 0;
            bool isBlank = TbxAmount.Text.Trim() == string.Empty;

            // Sets background color of textbox.
            TbxAmount.Background = (isValidText && isSaneAmount && isAboveZero) || isBlank ?
                new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);

            // Enables or disables credit button.
            BtnDeposit.IsEnabled = isValidText && isSaneAmount && isAboveZero && !isBlank;
        }

        private void UpdateLabelPerson()
        {
            LblPerson.Content = Person != null ? Person.ToString() : string.Empty;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void BtnDeposit_Click(object sender, RoutedEventArgs e)
        {
            double amount;

            if (!Double.TryParse(TbxAmount.Text.Trim(), out amount)) return;
            
            Deposit = new Deposit()
            {
                Amount = amount,
                PersonId = Person.Id,
                Comment = TbxComment.Text
            };
            DialogResult = true;
            return;
        }

        private void TbxAmount_TextChanged(object sender, TextChangedEventArgs e) => UpdateControlsOnValid();
    }
}
