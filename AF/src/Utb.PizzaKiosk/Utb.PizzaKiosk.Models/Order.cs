using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utb.PizzaKiosk.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required OrderStatusType OrderStatus { get; set; }
        public required FullfilmentOptionType FullfilmentOption { get; set; }
        public required decimal TotalPrice { get; set; }
    }
}
