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
        public Person Person { get; private set; }

        public double InitialDeposit { get; private set; } = 0;

        public AddPersonWindow()
        {
            InitializeComponent();
        }

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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            double initialDeposit;
            Person person = createPerson();

            if (person != null)
            {
                Person = person;
            }


            if(Double.TryParse(TbxInitialDeposit.Text, out initialDeposit))
            {
                InitialDeposit = initialDeposit;
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void TbxInitialDeposit_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = IsInitialDepositValid();
            TbxInitialDeposit.Background = isValid ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);
            
            // Why do I need the null check? Is it because it's initialized after the textbox?
            if (BtnAdd != null) BtnAdd.IsEnabled = isValid;
        }
    }
}
