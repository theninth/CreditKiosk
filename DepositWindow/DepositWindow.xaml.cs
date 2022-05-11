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
        }

        private void UpdateLabelPerson()
        {
            LblPerson.Content = Person != null ? Person.ToString() : string.Empty;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void BtnDeposit_Click(object sender, RoutedEventArgs e)
        {
            double amount;

            if (!Double.TryParse(TbxAmount.Text, out amount)) return;
            
            Deposit = new Deposit()
            {
                Amount = amount,
                PersonId = Person.Id,
                Comment = TbxComment.Text
            };
            DialogResult = true;
            return;
        }
    }
}
