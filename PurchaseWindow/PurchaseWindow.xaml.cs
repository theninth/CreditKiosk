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
        /// <summary>
        /// Used to make numpad create the right decimal seperator.
        /// </summary>
        private readonly string DECIMAL_SEPARATOR = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        private Person? person;
        private ProductGroup[]? productGroups;


        /// <summary>
        /// Person connected to this purchase.
        /// </summary>
        public Person? Person
        {
            get { return person; }
            set
            {
                person = value;
                UpdatePersonElements();
            }
        }

        /// <summary>
        /// Purchases provided for the windows parent to get purchases.
        /// </summary>
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
        
        /// <summary>
        /// Constructor.
        /// </summary>
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

        /// <summary>
        /// Adds an item as PurchaseItem to listview LvItems.
        /// </summary>
        /// <param name="productGroup">ProductGroup connected to the PurchaseItem</param>
        /// <param name="amount">Amount of item.</param>
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

            UpdateTotalLeftAfterPurchase();
            UpdateBtnPay();
        }

        /// <summary>
        /// Checks if TbxItemAmount is valid, if not, change textbox background and disable buttons.
        /// </summary>
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

        /// <summary>
        /// Calculate total cost of PurchseItems in listview LvItems.
        /// </summary>
        /// <returns></returns>
        private double CalcTotal()
        {
            return LvItems.Items.Cast<PurchaseItem>().ToList().Sum(i => i.Amount);
        }

        /// <summary>
        /// If TbxItemAmount.Text has text that can be parsed as a double, ask to make sure the user really want
        /// to exit window without finish the purchase.
        /// </summary>
        /// <returns></returns>
        private bool ContinueWithHalfPayment()
        {
            double _;

            if (Double.TryParse(TbxItemAmount.Text.Trim(), out _))
            {
                string message = "Du verkar ha skrivit i en summa utan att välja en varugrupp. " + Environment.NewLine +
                    "Vill du fortsätta utan att välja varugrupp för den inskrivna summan?";
                MessageBoxResult result = MessageBox.Show(message, "Betala?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                return result == MessageBoxResult.Yes;
            }
            return true;
        }

        /// <summary>
        /// Creates the Product group buttons according th the list in field productGroups.
        /// </summary>
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
                    button.IsEnabled = false;
                    button.MinHeight = 60;
                    button.MinWidth = 60;
                    button.Margin = new Thickness(5, 5, 5, 5);
                    button.Style = (Style)FindResource("MaterialDesignRaisedAccentButton");
                    ProductGroupButtons.Children.Add(button);
                }
            }
        }

        /// <summary>
        /// Enables/disables Delete button depending on if any item is selected.
        /// </summary>
        private void UpdateDeleteButton()
        {
            bool itemSelected = LvItems.SelectedIndex >= 0;
            BtnDelete.IsEnabled = itemSelected;
        }

        /// <summary>
        /// Update labels connected to person, like name, balance etc.
        /// </summary>
        private void UpdatePersonElements()
        {
            if (person == null) return;
            LblName.Content = $"{person.FirstName} {person.LastName}";
            LblBalance.Content = $"Saldo: {person.Balance:#,0.00} Kr";
        }

        /// <summary>
        /// Enables/disables BtnPay depending on state of other controls.
        /// </summary>
        private void UpdateBtnPay()
        {
            bool hasItems = LvItems.Items.Count > 0;
            bool amountIsEmpty = TbxItemAmount.Text.Trim() == String.Empty;
            BtnPay.IsEnabled = hasItems && amountIsEmpty;

        }

        /// <summary>
        /// Update label with total sum and label with left after purchase.
        /// </summary>
        private void UpdateTotalLeftAfterPurchase()
        {
            double total = CalcTotal();
            double leftAfterPurchase = Person != null ? Person.Balance - total : 0;
            LblTotal.Content = $"Totalt: {total:#,0.00} Kr.";
            LblLeftAfterPurchase.Content = $"Kvar efter köp: {leftAfterPurchase:#,0.00} Kr.";
            LblLeftAfterPurchase.Visibility = Visibility.Visible;
        }


        /*************************
         * CUSTOM EVENT HANDLERS * 
         *************************/

            /// <summary>
            /// Event handler for when numpad is pressed.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="e"></param>
            /// <exception cref="NotImplementedException"></exception>
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

        /*******************************
         * STANDARD GUI EVENT HANDLERS *
         *******************************/

        /// <summary>
        /// Event handler for when a productgroup button is pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Event handler for when text in TbxItemAmount changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void BtnAbort_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult frm = MessageBox.Show("Är du säker på att du vill avbryta", "Avbryta", MessageBoxButton.YesNo);
            if (frm == MessageBoxResult.Yes) this.Close();
        }

        /// <summary>
        /// Event handler for when Pay button is pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            if (!ContinueWithHalfPayment()) return;

            List<PurchaseItem> items = LvItems.Items.Cast<PurchaseItem>().ToList();
            List<Purchase> purchases = PurchaseHelpers.PurchasesGroupedByProduceGroup(items);

            foreach (Purchase purchase in purchases)
            {
                purchase.PersonId = Person.Id;
            }

            Purchases = purchases;
            this.Close();
        }

        /// <summary>
        /// Event handler for when Delete button is pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LvItems.SelectedIndex >= 0)
            {
                LvItems.Items.RemoveAt(LvItems.SelectedIndex);
                UpdateTotalLeftAfterPurchase();
            }
        }

        /// <summary>
        /// Event handler for when selection in listview LvItems is changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LvItems_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateDeleteButton();

        /// <summary>
        /// Event handler for when text in TbxItemAmount changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void TbxItemAmount_TextChanged(object sender, TextChangedEventArgs e) => CheckInvalidAmount();
    }
}

