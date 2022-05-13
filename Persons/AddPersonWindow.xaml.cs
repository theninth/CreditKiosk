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

namespace CreditKiosk.Persons
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
        /// <summary>
        /// Person object to be used by the window's parent.
        /// </summary>
        public Person Person { get; private set; }

        /// <summary>
        /// Amount for the initial deposit to be used by the window's parent.
        /// </summary>
        public double InitialDeposit { get; private set; } = 0;

        public AddPersonWindow()
        {
            InitializeComponent();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        /// <summary>
        /// Create Person object from values in form.
        /// </summary>
        /// <returns>Person created.</returns>
        private Person? createPerson()
        {
            Person person = new Person();
            string firstName = TbxFirstName.Text.Trim();
            string lastName = TbxLastName.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                string message = "Du måste ange förnamn och efternamn.";
                MessageBox.Show(message, "Fel!");
                return null;
            }

            person.FirstName = firstName;
            person.LastName = lastName;
            return person;
        }

        /// <summary>
        /// Makes sure initial deposit is valid.
        /// </summary>
        /// <returns>Validity</returns>
        private bool IsInitialDepositValid()
        {
            double value;
            string initialDepositText = TbxInitialDeposit.Text.Trim();

            bool successParse = Double.TryParse(initialDepositText, out value);
            bool isNotNegative = successParse && value >= 0;

            return successParse && isNotNegative;
        }

        /// <summary>
        /// Event handler for Button add click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            double initialDeposit;
            Person person = createPerson();

            if (person == null) return;

            Person = person;


            if(Double.TryParse(TbxInitialDeposit.Text, out initialDeposit))
            {
                InitialDeposit = initialDeposit;
            }

            this.Close();
        }

        /// <summary>
        /// Event handler for Button cancel click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Event handler for when text in initial deposit text box is changed. This method
        /// validates that text box.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void TbxInitialDeposit_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = IsInitialDepositValid();
            TbxInitialDeposit.Background = isValid ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);
            
            // Why do I need the null check? Is it because it's initialized after the textbox?
            if (BtnAdd != null) BtnAdd.IsEnabled = isValid;
        }
    }
}
