using CreditKiosk.Events;
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

namespace CreditKiosk.ProductGroups
{
    /// <summary>
    /// Interaction logic for ProductGroupsWindow.xaml
    /// </summary>
    public partial class ProductGroupsWindow : Window
    {
        public ProductGroupsWindow(IEnumerable<ProductGroup> productGroups)
        {
            InitializeComponent();
            SetDataLvProductGroups(productGroups);

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }

        /// <summary>
        /// Subscribe to this event to watch when a new product group is added.
        /// </summary>
        public event EventHandler<ProductGroupEventArgs>? ProductGroupAdded;

        /// <summary>
        /// Subscribe to this event to watch when a new product group is deleted.
        /// </summary>
        public event EventHandler<ProductGroupEventArgs>? ProductGroupDeleted;


        private void SetDataLvProductGroups(IEnumerable<ProductGroup> productGroups)
        {
            LvProductGroups.Items.Clear();
            foreach (ProductGroup productGroup in productGroups)
            {
                LvProductGroups.Items.Add(productGroup);
            }
        }

        protected virtual void OnProductGroupAdded(ProductGroupEventArgs e) => ProductGroupAdded?.Invoke(this, e);

        protected virtual void OnProductGroupDeleted(ProductGroupEventArgs e) => ProductGroupDeleted?.Invoke(this, e);

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductGroupAddWindow frm = new();
            frm.ShowDialog();

            if (frm.ProductGroup != null)
            {
                ProductGroupEventArgs productGroupEventArgs = new ProductGroupEventArgs(frm.ProductGroup);
                OnProductGroupAdded(productGroupEventArgs);
                LvProductGroups.Items.Add(frm.ProductGroup);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxImage icon;
            string message;
            string title = "Är du säker?";

            ProductGroup? productGroup = LvProductGroups.SelectedItem as ProductGroup;

            if (productGroup == null) return;

            if (productGroup.Total > 0)
            {
                message = "Produktgruppen innehåller varor, om du raderar den kommer även de kopplade köpen försvinna." +
                    Environment.NewLine + Environment.NewLine +
                    "Är du helt säker på att du vill radera den?";
                icon = MessageBoxImage.Warning;
            }
            else
            {
                message = "Är du säker på att du vill radera produktgruppen (inga köp försvinner)?";
                icon = MessageBoxImage.Information;
            }

            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, icon);
            if (result == MessageBoxResult.Yes)
            {
                ProductGroupEventArgs productGroupEventArgs = new ProductGroupEventArgs(productGroup);
                OnProductGroupDeleted(productGroupEventArgs);

                LvProductGroups.Items.Remove(productGroup);

            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
