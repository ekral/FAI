using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class BooleanValue : SelectedValue
    {
        public int BooleanOptionId { get; set; }
        public BooleanOption? BooleanOption { get; set; }
        public required bool IsSelected { get; set; } 
    }
}
