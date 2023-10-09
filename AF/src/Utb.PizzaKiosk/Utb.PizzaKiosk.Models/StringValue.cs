using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class StringValue : SelectedValue
    {
        public int StringOptionsId { get; set; }

        public StringOptions? StringOptions { get; set; }

        public required string SelectedString { get; set; }
    }
}
