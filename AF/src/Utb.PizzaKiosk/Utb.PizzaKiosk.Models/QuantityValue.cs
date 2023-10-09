using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class QuantityValue : SelectedValue
    {
        public int QuantityOptionId { get; set; }
        public QuantityOption? QuantityOption { get; set; }
        public required int Quantity { get; set; }
    }
}
