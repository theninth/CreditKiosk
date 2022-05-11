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
    /// Interaction logic for ProductGroupAddWindow.xaml
    /// </summary>
    public partial class ProductGroupAddWindow : Window
    {
        public ProductGroup? ProductGroup { get; set; }
        
        public ProductGroupAddWindow()
        {
            InitializeComponent();

            // Run fullscreen in production.
#if !DEBUG
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
#endif
        }


        private bool isNameValid()
        {
            return TbxName.Text.Trim().Length > 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TbxName.Text.Trim();

            ProductGroup productGroup = new ProductGroup
            {
                Name = name
            };

            ProductGroup = productGroup;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TbxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = isNameValid();
            TbxName.Background = isValid ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Pink);
            BtnAdd.IsEnabled = isValid;
        }
    }
}
