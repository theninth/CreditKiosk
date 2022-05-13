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
        /// <summary>
        /// Person connected to the deposit.
        /// </summary>
        private Person Person { get; set; }

        /// <summary>
        /// Deposit object to be returned.
        /// </summary>
        public Deposit? Deposit { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="person">Person connected to the deposit</param>
        /// <exception cref="ArgumentNullException">If person is null</exception>
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

        /// <summary>
        /// Sets a few properties on the controls depending on if the amount in textbox
        /// is valid or not.
        /// </summary>
        private void UpdateControlsOnValid()
        {
            double amount;  // Will hold amount when it's parsed from the textbox.

            // Text parseable to a double.
            bool isValidText = Double.TryParse(TbxAmount.Text.Trim(), out amount);

            // No large values allowed (that wouldn't make sense for this app)
            bool isSaneAmount = amount <= (double)App.Current.Resources["SaneAmount"];

            // Is amount above zero.
            bool isAboveZero = amount > 0;

            // Is textbox blank (empty or with whitespaces only)
            bool isBlank = TbxAmount.Text.Trim() == string.Empty;

            // Sets background color of textbox to white if valid, pink if not.
            TbxAmount.Background = (isValidText && isSaneAmount && isAboveZero) || isBlank ?
                new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);

            // Enables or disables credit button depending if the value is 
            BtnDeposit.IsEnabled = isValidText && isSaneAmount && isAboveZero && !isBlank;
        }

        /// <summary>
        /// Sets value to the person name label.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>

        private void UpdateLabelPerson()
        {
            LblPerson.Content = Person != null ? Person.ToString() : string.Empty;
        }

        /// <summary>
        /// Exit on cancel.
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Deposit clicked.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
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

        /// <summary>
        /// On text changed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void TbxAmount_TextChanged(object sender, TextChangedEventArgs e) => UpdateControlsOnValid();
    }
}
