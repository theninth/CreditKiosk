using CreditKiosk.Events;
using CreditKiosk.Models;
using CreditKiosk.PurchaseWindow.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CreditKiosk.PurchaseWindow
{
    /// <summary>
    /// Interaction logic for PurchaseWindow.xaml
    /// </summary>
    public partial class PurchaseWindow : Window
    {
        private readonly string DECIMAL_SEPARATOR = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        private Person? person;
        private ProductGroup[]? productGroups;

        public Person? Person
        {
            get { return person; }
            set
            {
                person = value;
                UpdatePersonElements();
            }
        }

        public List<Purchase>? Purchases;

        /// <summary>
        /// You have to set these to the product groups that are available to the purchase.
        /// </summary>
        public ProductGroup[]? ProductGroups
        {
            get
            { return this.productGroups; }
            set
            {
                this.productGroups = value;
                UpdateProductGroupButtons();
            }
        }
        
        public PurchaseWindow()
        {
            InitializeComponent();
            this.NumPadAmount.NumPadPressed += OnNumPadPressed;

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        private void AddItem(ProductGroup productGroup, double amount)
        {
            PurchaseItem item;
            
            try
            {
                item = new PurchaseItem(productGroup, amount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                return;
            }

            LvItems.Items.Add(item);
            TbxItemAmount.Text = String.Empty;

            UpdateTotal();
            UpdateBtnPay();
        }

        private void CheckInvalidAmount()
        {
            double amount;
            bool isEmpty = TbxItemAmount.Text.Trim() == String.Empty;
            bool isValid = Double.TryParse(TbxItemAmount.Text.Trim(), out amount) && amount > 0;
            
            TbxItemAmount.Background = isEmpty || isValid ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);
            foreach (Button button in ProductGroupButtons.Children)
            {
                button.IsEnabled = isValid;
            }
        }

        private double CalcTotal()
        {
            return LvItems.Items.Cast<PurchaseItem>().ToList().Sum(i => i.Amount);
        }

        private void UpdateProductGroupButtons()
        {
            ProductGroupButtons.Children.Clear();
            if (productGroups == null) return;

            foreach (ProductGroup group in productGroups)
            {
                if (group != null)
                {
                    ProductGroupButton button = new();
                    button.Content = group.Name;
                    button.ConnectedProductGroup = group;
                    button.Click += BtnProductGroup_Click;
                    button.MinHeight = 60;
                    button.MinWidth = 60;
                    button.Margin = new Thickness(5, 5, 5, 5);
                    ProductGroupButtons.Children.Add(button);
                }
            }
        }

        private void UpdateDeleteButton()
        {
            bool itemSelected = LvItems.SelectedIndex >= 0;
            BtnDelete.IsEnabled = itemSelected;
        }

        private void UpdatePersonElements()
        {
            if (person == null) return;
            LblName.Content = $"{person.FirstName} {person.LastName}";
            LblBalance.Content = $"Saldo: {person.Balance:#,0.00} Kr";
        }

        private void UpdateBtnPay()
        {
            BtnPay.IsEnabled = LvItems.Items.Count > 0;
        }

        private void UpdateTotal() => LblTotal.Content = $"Totalt: {CalcTotal():#,0.00} Kr.";

        private void OnNumPadPressed(object source, NumPadPressedEventArgs e)
        {
            switch (e.NumPadButton)
            {
                case Enums.NumPadButtons.Digit:
                    TbxItemAmount.Text += e.Digit;
                    break;
                case Enums.NumPadButtons.DecimalSeparator:
                    TbxItemAmount.Text += DECIMAL_SEPARATOR;
                    break;
                case Enums.NumPadButtons.Clear:
                    TbxItemAmount.Text = "";
                    break;
                default:
                    throw new NotImplementedException("This enum value was not implemented. This should not happen.");
            }
        }

        private void BtnProductGroup_Click(object sender, RoutedEventArgs e)
        {
            double amount;
            ProductGroupButton buttonClicked = (ProductGroupButton)sender;

            if (buttonClicked.ConnectedProductGroup == null)
            {
                throw new ArgumentException("No product group supplied to button. Add to property 'ConnectedProductGroup'");
            }

            ProductGroup productGroup = buttonClicked.ConnectedProductGroup;

            if (!Double.TryParse(TbxItemAmount.Text.Trim(), out amount))
            {
                // Detta ska inte kunna hända, men lika bra att vara säker.
                MessageBox.Show("Ogiltigt summa!", "Fel!");
                return;
            }
            
            if (person != null && amount <= person.Balance)
            {
                AddItem(productGroup, amount);
            }
            else
            {
                MessageBox.Show("Du har inte tillräckligt med pengar kvar!", "Fel!");
            }
        }

        private void TbxItemAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInvalidAmount();
        }

        private void BtnAbort_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult frm = MessageBox.Show("Är du säker på att du vill avbryta", "Avbryta", MessageBoxButton.YesNo);
            if (frm == MessageBoxResult.Yes) this.Close();
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            List<PurchaseItem> items = LvItems.Items.Cast<PurchaseItem>().ToList();
            List<Purchase> purchases = PurchaseHelpers.PurchasesGroupedByProduceGroup(items);

            foreach (Purchase purchase in purchases)
            {
                purchase.PersonId = Person.Id;
            }

            Purchases = purchases;
            this.Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LvItems.SelectedIndex >= 0)
            {
                LvItems.Items.RemoveAt(LvItems.SelectedIndex);
                UpdateTotal();
            }
        }

        private void LvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDeleteButton();
        }
    }
}

