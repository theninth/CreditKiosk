using CreditKiosk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CreditKiosk.PurchaseWindow
{
    internal class ProductGroupButton : Button
    {
        public ProductGroup? ConnectedProductGroup { get; set; }
    }
}
