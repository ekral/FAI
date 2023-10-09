using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class BooleanOption : PizzaConfigurationOption
    {
        public bool DefaultValue { get; set; }
        public bool IsDefault(bool value) => value == DefaultValue;
    }
}
