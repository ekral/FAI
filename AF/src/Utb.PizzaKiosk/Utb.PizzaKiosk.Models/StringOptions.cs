using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    // https://www.conradakunga.com/blog/saving-collections-of-primitives-in-entity-framework-core/

    public class StringOptions : PizzaConfigurationOption
    {
        public required ICollection<string> Options { get; set; }
        public required int DefaultValueIndex { get; set; }
        public bool IsDefault(string value) => true;

    }
}
